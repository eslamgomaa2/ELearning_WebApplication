using E_Learning.Service.DTOs.Payment;

namespace E_Learning.Service.Contract;
public interface IPaymentService
{
    // ── Stripe ──────────────────────────────────────────
    Task<CreatePaymentIntentResponseDto> CreatePaymentIntentAsync(
        CreatePaymentIntentRequestDto dto,
        Guid studentId,
        CancellationToken ct = default);

    Task<ConfirmPaymentResponseDto> ConfirmPaymentAsync(
        ConfirmPaymentRequestDto dto,
        Guid studentId,
        CancellationToken ct = default);

    Task HandleStripeWebhookAsync(
        string payload,
        string stripeSignature,
        CancellationToken ct = default);

    // ── Payment Methods ──────────────────────────────────
    Task<PaymentMethodResponseDto> AddPaymentMethodAsync(
        AddPaymentMethodRequestDto dto,
        Guid userId,
        CancellationToken ct = default);

    Task<List<PaymentMethodResponseDto>> GetPaymentMethodsAsync(
        Guid userId,
        CancellationToken ct = default);

    Task DeletePaymentMethodAsync(
        int paymentMethodId,
        Guid userId,
        CancellationToken ct = default);

    Task SetDefaultPaymentMethodAsync(
        int paymentMethodId,
        Guid userId,
        CancellationToken ct = default);

    // ── Transactions ─────────────────────────────────────
    Task<List<PaymentTransactionResponseDto>> GetMyTransactionsAsync(
        Guid studentId,
        CancellationToken ct = default);

    // ── Instructor Earnings ──────────────────────────────
    Task<InstructorEarningsResponseDto> GetMyEarningsAsync(
        Guid instructorId,
        CancellationToken ct = default);

    Task<PayoutRequestResponseDto> CreatePayoutRequestAsync(
        CreatePayoutRequestDto dto,
        Guid instructorId,
        CancellationToken ct = default);

    Task<List<PayoutRequestResponseDto>> GetMyPayoutRequestsAsync(
        Guid instructorId,
        CancellationToken ct = default);

    // ── Admin ────────────────────────────────────────────
    Task<List<PayoutRequestResponseDto>> GetPendingPayoutRequestsAsync(
        CancellationToken ct = default);

    Task ProcessPayoutRequestAsync(
        ProcessPayoutRequestDto dto,
        Guid adminId,
        CancellationToken ct = default);
}