using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Entities.Enrollment;
using E_Learning.Core.Enums;
using E_Learning.Core.Exceptions;
using E_Learning.Core.Repository;
using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Payment;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Events;
using E_Learning.Core.Interfaces.Repositories.Payments;

namespace E_Learning.Service.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _config;
    private readonly decimal _platformFeePercent;

    public PaymentService(IUnitOfWork uow, IConfiguration config)
    {
        _uow = uow;
        _config = config;
        _platformFeePercent = decimal.Parse(
            config["Stripe:PlatformFeePercent"] ?? "20");

        StripeConfiguration.ApiKey = config["Stripe:SecretKey"]!;
    }

    // ═══════════════════════════════════════════════════════════
    // CREATE PAYMENT INTENT
    // ═══════════════════════════════════════════════════════════
    public async Task<CreatePaymentIntentResponseDto> CreatePaymentIntentAsync(
        CreatePaymentIntentRequestDto dto,
        Guid studentId,
        CancellationToken ct = default)
    {
        // 1. Get course
        var course = await _uow.Courses.GetByIdAsync(dto.CourseId, ct)
            ?? throw new NotFoundException("Course not found");

        if (course.Price <= 0)
            throw new BadRequestException("This course is free — no payment needed");

        // 2. Check not already enrolled
        var alreadyEnrolled = await _uow.Enrollments.AnyAsync(
            e => e.StudentId == studentId && e.CourseId == dto.CourseId, ct);
        if (alreadyEnrolled)
            throw new BadRequestException("You are already enrolled in this course");

        // 3. Create Stripe PaymentIntent
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(course.Price * 100), // Stripe uses cents
            Currency = "usd",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            // For test only - in production handle by frontend team stripe.json
            PaymentMethod = "pm_card_visa",
            Confirm = true,          // ← Auto confirm
            ReturnUrl = "https://localhost:7000/payment-success", 
            Metadata = new Dictionary<string, string>
            {
                { "courseId",  course.Id.ToString() },
                { "studentId", studentId.ToString() }
            }
        };

        // Attach saved payment method if provided
        if (dto.PaymentMethodId.HasValue)
        {
            var savedMethod = await _uow.PaymentMethods
                .GetByIdAsync(dto.PaymentMethodId.Value, ct)
                ?? throw new NotFoundException("Payment method not found");

            if (savedMethod.UserId != studentId)
                throw new ForbiddenException("Not your payment method");

            options.PaymentMethod = savedMethod.StripePaymentMethodId;
        }

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return new CreatePaymentIntentResponseDto
        {
            ClientSecret = intent.ClientSecret,
            PaymentIntentId = intent.Id,
            Amount = course.Price,
            Currency = "USD"
        };
    }

    // ═══════════════════════════════════════════════════════════
    // CONFIRM PAYMENT
    // ═══════════════════════════════════════════════════════════
    public async Task<ConfirmPaymentResponseDto> ConfirmPaymentAsync(
        ConfirmPaymentRequestDto dto,
        Guid studentId,
        CancellationToken ct = default)
    {
        // 1. Verify with Stripe
        var service = new PaymentIntentService();
        var intent = await service.GetAsync(dto.PaymentIntentId);

        if (intent.Status != "succeeded" && intent.Status != "requires_capture")
            throw new BadRequestException($"Payment not completed. Status: {intent.Status}");

        // 2. Check not duplicate
        var existing = await _uow.PaymentTransactions
            .GetByGatewayReferenceAsync(dto.PaymentIntentId, ct);
        if (existing is not null)
            throw new BadRequestException("Payment already processed");

        // 3. Get course + instructor
        var course = await _uow.Courses.GetByIdAsync(dto.CourseId, ct)
            ?? throw new NotFoundException("Course not found");

        await _uow.BeginTransactionAsync(ct);
        try
        {
            // 4. Save transaction
            var transaction = new PaymentTransaction
            {
                StudentId = studentId,
                CourseId = dto.CourseId,
                PaymentMethodId = dto.PaymentMethodId,
                Amount = course.Price,
                Currency = "USD",
                Status = PaymentStatus.Completed,
                GatewayReference = dto.PaymentIntentId,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };
            await _uow.PaymentTransactions.AddAsync(transaction, ct);
            await _uow.SaveChangesAsync(ct);

            // 5. Create enrollment
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = dto.CourseId,
                TransactionId = transaction.Id,
                Status = EnrollmentStatus.NotStarted,
                ProgressPercentage = 0,
                EnrolledAt = DateTime.UtcNow
            };
            await _uow.Enrollments.AddAsync(enrollment, ct);

            // 6. Calculate and save instructor earning
            var platformFee = course.Price * (_platformFeePercent / 100);
            var netAmount = course.Price - platformFee;

            var earning = new InstructorEarning
            {
                InstructorId = course.InstructorId,
                TransactionId = transaction.Id,
                CourseId = dto.CourseId,
                GrossAmount = course.Price,
                PlatformFee = platformFee,
                NetAmount = netAmount,
                Status = EarningStatus.Pending,
                AvailableAt = DateTime.UtcNow.AddDays(14), // 14-day hold
                CreatedAt = DateTime.UtcNow
            };
            await _uow.InstructorEarnings.AddAsync(earning, ct);

            // 7. Save card if requested
            if (dto.SaveCard && dto.PaymentMethodId.HasValue)
            {
                // Already saved — just mark as default if needed
            }

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return new ConfirmPaymentResponseDto
            {
                Success = true,
                Message = "Payment successful! You are now enrolled.",
                EnrollmentId = enrollment.Id,
                TransactionId = transaction.Id
            };
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // STRIPE WEBHOOK
    // ═══════════════════════════════════════════════════════════
    public async Task HandleStripeWebhookAsync(
        string payload,
        string stripeSignature,
        CancellationToken ct = default)
    {
        var webhookSecret = _config["Stripe:WebhookSecret"]!;
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                payload, stripeSignature, webhookSecret);
        }
        catch
        {
            throw new BadRequestException("Invalid Stripe webhook signature");
        }

        // Handle payment_intent.succeeded
        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent is null) return;

            // Update transaction status if exists
            var transaction = await _uow.PaymentTransactions
                .GetByGatewayReferenceAsync(intent.Id, ct);

            if (transaction is not null && transaction.Status != PaymentStatus.Completed)
            {
                transaction.Status = PaymentStatus.Completed;
                transaction.CompletedAt = DateTime.UtcNow;
                _uow.PaymentTransactions.Update(transaction);
                await _uow.SaveChangesAsync(ct);
            }
        }

        // Handle payment_intent.payment_failed
        if (stripeEvent.Type == "payment_intent.payment_failed")
        {
            var intent = stripeEvent.Data.Object as PaymentIntent;
            if (intent is null) return;

            var transaction = await _uow.PaymentTransactions
                .GetByGatewayReferenceAsync(intent.Id, ct);

            if (transaction is not null)
            {
                transaction.Status = PaymentStatus.Failed;
                transaction.FailureReason = intent.LastPaymentError?.Message;
                _uow.PaymentTransactions.Update(transaction);
                await _uow.SaveChangesAsync(ct);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // PAYMENT METHODS
    // ═══════════════════════════════════════════════════════════
    public async Task<PaymentMethodResponseDto> AddPaymentMethodAsync(
        AddPaymentMethodRequestDto dto,
        Guid userId,
        CancellationToken ct = default)
    {
        string? cardLastFour = null;
        string? cardHolder = null;
        byte? expiryMonth = null;
        short? expiryYear = null;
        string? stripeMethodId = dto.StripePaymentMethodId;

        // Fetch card details from Stripe
        if (dto.Type == "CreditCard" && dto.StripePaymentMethodId is not null)
        {
            var pmService = new PaymentMethodService();
            var pm = await pmService.GetAsync(dto.StripePaymentMethodId);

            cardLastFour = pm.Card?.Last4;
            cardHolder = pm.BillingDetails?.Name;
            expiryMonth = pm.Card?.ExpMonth != null ? (byte)pm.Card.ExpMonth : null;
            expiryYear = pm.Card?.ExpYear != null ? (short)pm.Card.ExpYear : null;
        }

        // Clear existing default if needed
        if (dto.IsDefault)
            await _uow.PaymentMethods.ClearDefaultAsync(userId, ct);

        var method = new Core.Entities.Billing.PaymentMethod
        {
            UserId = userId,
            Type = dto.Type,
            StripePaymentMethodId = stripeMethodId,
            CardLastFour = cardLastFour,
            CardHolderName = cardHolder,
            ExpiryMonth = expiryMonth,
            ExpiryYear = expiryYear,
            PayPalEmail = dto.PayPalEmail,
            IsDefault = dto.IsDefault,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.PaymentMethods.AddAsync(method, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToPaymentMethodDto(method);
    }

    public async Task<List<PaymentMethodResponseDto>> GetPaymentMethodsAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var methods = await _uow.PaymentMethods
            .GetUserPaymentMethodsAsync(userId, ct);

        return methods.Select(MapToPaymentMethodDto).ToList();
    }

    public async Task DeletePaymentMethodAsync(
        int paymentMethodId,
        Guid userId,
        CancellationToken ct = default)
    {
        var method = await _uow.PaymentMethods.GetByIdAsync(paymentMethodId, ct)
            ?? throw new NotFoundException("Payment method not found");

        if (method.UserId != userId)
            throw new ForbiddenException("Not your payment method");

        _uow.PaymentMethods.Remove(method);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task SetDefaultPaymentMethodAsync(
        int paymentMethodId,
        Guid userId,
        CancellationToken ct = default)
    {
        var method = await _uow.PaymentMethods.GetByIdAsync(paymentMethodId, ct)
            ?? throw new NotFoundException("Payment method not found");

        if (method.UserId != userId)
            throw new ForbiddenException("Not your payment method");

        await _uow.PaymentMethods.ClearDefaultAsync(userId, ct);

        method.IsDefault = true;
        _uow.PaymentMethods.Update(method);
        await _uow.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════
    // TRANSACTIONS
    // ═══════════════════════════════════════════════════════════
    public async Task<List<PaymentTransactionResponseDto>> GetMyTransactionsAsync(
        Guid studentId,
        CancellationToken ct = default)
    {
        var transactions = await _uow.PaymentTransactions
            .GetStudentTransactionsAsync(studentId, ct);

        return transactions.Select(t => new PaymentTransactionResponseDto
        {
            Id = t.Id,
            CourseName = t.Course.Title,
            CourseThumbnail = t.Course.ThumbnailUrl ?? string.Empty,
            Amount = t.Amount,
            Currency = t.Currency,
            Status = t.Status.ToString(),
            GatewayReference = t.GatewayReference,
            CreatedAt = t.CreatedAt,
            CompletedAt = t.CompletedAt
        }).ToList();
    }

    // ═══════════════════════════════════════════════════════════
    // INSTRUCTOR EARNINGS
    // ═══════════════════════════════════════════════════════════
    public async Task<InstructorEarningsResponseDto> GetMyEarningsAsync(
        Guid instructorId,
        CancellationToken ct = default)
    {
        var earnings = await _uow.InstructorEarnings
            .GetInstructorEarningsAsync(instructorId, ct);

        var available = await _uow.InstructorEarnings
            .GetAvailableBalanceAsync(instructorId, ct);

        return new InstructorEarningsResponseDto
        {
            TotalEarnings = earnings.Sum(e => e.NetAmount),
            PendingPayout = earnings
                .Where(e => e.Status == EarningStatus.Pending)
                .Sum(e => e.NetAmount),
            AvailableForPayout = available,
            Earnings = earnings.Select(e => new EarningItemDto
            {
                Id = e.Id,
                CourseName = e.Course.Title,
                GrossAmount = e.GrossAmount,
                PlatformFee = e.PlatformFee,
                NetAmount = e.NetAmount,
                Status = e.Status.ToString(),
                AvailableAt = e.AvailableAt,
                CreatedAt = e.CreatedAt
            }).ToList()
        };
    }

    // ═══════════════════════════════════════════════════════════
    // PAYOUT REQUESTS
    // ═══════════════════════════════════════════════════════════
    public async Task<PayoutRequestResponseDto> CreatePayoutRequestAsync(
        CreatePayoutRequestDto dto,
        Guid instructorId,
        CancellationToken ct = default)
    {
        // 1. Check no pending request
        var hasPending = await _uow.PayoutRequests
            .HasPendingRequestAsync(instructorId, ct);
        if (hasPending)
            throw new BadRequestException("You already have a pending payout request");

        // 2. Check available balance
        var available = await _uow.InstructorEarnings
            .GetAvailableBalanceAsync(instructorId, ct);
        if (dto.Amount > available)
            throw new BadRequestException(
                $"Insufficient balance. Available: {available:F2} USD");

        // 3. Create request
        var request = new PayoutRequest
        {
            InstructorId = instructorId,
            Amount = dto.Amount,
            Method = dto.Method,
            AccountDetails = dto.AccountDetails,
            Status = PayoutStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };

        await _uow.PayoutRequests.AddAsync(request, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToPayoutDto(request);
    }

    public async Task<List<PayoutRequestResponseDto>> GetMyPayoutRequestsAsync(
        Guid instructorId,
        CancellationToken ct = default)
    {
        var requests = await _uow.PayoutRequests
            .GetInstructorPayoutsAsync(instructorId, ct);

        return requests.Select(MapToPayoutDto).ToList();
    }

    // ═══════════════════════════════════════════════════════════
    // ADMIN — PAYOUT MANAGEMENT
    // ═══════════════════════════════════════════════════════════
    public async Task<List<PayoutRequestResponseDto>> GetPendingPayoutRequestsAsync(
        CancellationToken ct = default)
    {
        var requests = await _uow.PayoutRequests.GetPendingPayoutsAsync(ct);
        return requests.Select(MapToPayoutDto).ToList();
    }

    public async Task ProcessPayoutRequestAsync(
        ProcessPayoutRequestDto dto,
        Guid adminId,
        CancellationToken ct = default)
    {
        var request = await _uow.PayoutRequests.GetByIdAsync(dto.PayoutRequestId, ct)
            ?? throw new NotFoundException("Payout request not found");

        if (request.Status != PayoutStatus.Pending)
            throw new BadRequestException("Request already processed");

        await _uow.BeginTransactionAsync(ct);
        try
        {
            // Update request status
            request.Status = dto.Decision == "Approved"
                ? PayoutStatus.Approved
                : PayoutStatus.Rejected;
            request.ProcessedAt = DateTime.UtcNow;
            request.AdminNotes = dto.Notes;
            _uow.PayoutRequests.Update(request);

            // Save approval record
            var approval = new PayoutApproval
            {
                PayoutRequestId = request.Id,
                AdminId = adminId,
                Decision = dto.Decision,
                Notes = dto.Notes,
                ProcessedAt = DateTime.UtcNow
            };
            await _uow.PayoutApprovals.AddAsync(approval, ct);

            // If approved — mark earnings as PaidOut
            if (dto.Decision == "Approved")
            {
                var earnings = await _uow.InstructorEarnings
                    .GetInstructorEarningsAsync(request.InstructorId, ct);

                decimal remaining = request.Amount;
                foreach (var earning in earnings
                    .Where(e => e.Status == EarningStatus.Available)
                    .OrderBy(e => e.CreatedAt))
                {
                    if (remaining <= 0) break;

                    earning.Status = EarningStatus.PaidOut;
                    _uow.InstructorEarnings.Update(earning);
                    remaining -= earning.NetAmount;
                }
            }

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // PRIVATE HELPERS
    // ═══════════════════════════════════════════════════════════
    private static PaymentMethodResponseDto MapToPaymentMethodDto(
        Core.Entities.Billing.PaymentMethod pm) => new()
        {
            Id = pm.Id,
            Type = pm.Type,
            CardLastFour = pm.CardLastFour,
            CardHolderName = pm.CardHolderName,
            ExpiryMonth = pm.ExpiryMonth,
            ExpiryYear = pm.ExpiryYear,
            PayPalEmail = pm.PayPalEmail,
            IsDefault = pm.IsDefault
        };

    private static PayoutRequestResponseDto MapToPayoutDto(PayoutRequest pr) => new()
    {
        Id = pr.Id,
        Amount = pr.Amount,
        Method = pr.Method,
        Status = pr.Status.ToString(),
        RequestedAt = pr.RequestedAt,
        ProcessedAt = pr.ProcessedAt,
        AdminNotes = pr.AdminNotes
    };
}