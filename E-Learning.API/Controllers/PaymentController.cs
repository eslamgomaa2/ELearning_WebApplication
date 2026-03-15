using E_Learning.Service.Contract;
using E_Learning.Service.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Learning.API.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
        => _paymentService = paymentService;

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── Stripe ──────────────────────────────────────────────

    /// POST api/payments/create-intent
    [HttpPost("create-intent")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> CreatePaymentIntent(
        [FromBody] CreatePaymentIntentRequestDto dto,
        CancellationToken ct)
    {
        var result = await _paymentService
            .CreatePaymentIntentAsync(dto, CurrentUserId, ct);
        return Ok(result);
    }

    /// POST api/payments/confirm
    [HttpPost("confirm")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> ConfirmPayment(
        [FromBody] ConfirmPaymentRequestDto dto,
        CancellationToken ct)
    {
        var result = await _paymentService
            .ConfirmPaymentAsync(dto, CurrentUserId, ct);
        return Ok(result);
    }

    /// POST api/payments/webhook
    [HttpPost("webhook")]
    [AllowAnonymous] // Stripe calls this directly
    public async Task<IActionResult> StripeWebhook(CancellationToken ct)
    {
        var payload = await new StreamReader(Request.Body).ReadToEndAsync(ct);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        await _paymentService.HandleStripeWebhookAsync(payload, signature, ct);
        return Ok();
    }

    // ── Payment Methods ─────────────────────────────────────

    /// GET api/payments/methods
    [HttpGet("methods")]
    public async Task<IActionResult> GetPaymentMethods(CancellationToken ct)
    {
        var result = await _paymentService
            .GetPaymentMethodsAsync(CurrentUserId, ct);
        return Ok(result);
    }

    /// POST api/payments/methods
    [HttpPost("methods")]
    public async Task<IActionResult> AddPaymentMethod(
        [FromBody] AddPaymentMethodRequestDto dto,
        CancellationToken ct)
    {
        var result = await _paymentService
            .AddPaymentMethodAsync(dto, CurrentUserId, ct);
        return Ok(result);
    }

    /// DELETE api/payments/methods/{id}
    [HttpDelete("methods/{id:int}")]
    public async Task<IActionResult> DeletePaymentMethod(
        int id,
        CancellationToken ct)
    {
        await _paymentService.DeletePaymentMethodAsync(id, CurrentUserId, ct);
        return Ok(new { message = "Payment method removed" });
    }

    /// PATCH api/payments/methods/{id}/set-default
    [HttpPatch("methods/{id:int}/set-default")]
    public async Task<IActionResult> SetDefaultPaymentMethod(
        int id,
        CancellationToken ct)
    {
        await _paymentService.SetDefaultPaymentMethodAsync(id, CurrentUserId, ct);
        return Ok(new { message = "Default payment method updated" });
    }

    // ── Transactions ────────────────────────────────────────

    /// GET api/payments/transactions
    [HttpGet("transactions")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyTransactions(CancellationToken ct)
    {
        var result = await _paymentService
            .GetMyTransactionsAsync(CurrentUserId, ct);
        return Ok(result);
    }

    // ── Instructor Earnings ─────────────────────────────────

    /// GET api/payments/earnings
    [HttpGet("earnings")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetMyEarnings(CancellationToken ct)
    {
        var result = await _paymentService
            .GetMyEarningsAsync(CurrentUserId, ct);
        return Ok(result);
    }

    /// POST api/payments/payout-requests
    [HttpPost("payout-requests")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> CreatePayoutRequest(
        [FromBody] CreatePayoutRequestDto dto,
        CancellationToken ct)
    {
        var result = await _paymentService
            .CreatePayoutRequestAsync(dto, CurrentUserId, ct);
        return Ok(result);
    }

    /// GET api/payments/payout-requests
    [HttpGet("payout-requests")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetMyPayoutRequests(CancellationToken ct)
    {
        var result = await _paymentService
            .GetMyPayoutRequestsAsync(CurrentUserId, ct);
        return Ok(result);
    }

    // ── Admin ───────────────────────────────────────────────

    /// GET api/payments/admin/payout-requests
    [HttpGet("admin/payout-requests")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPendingPayoutRequests(CancellationToken ct)
    {
        var result = await _paymentService
            .GetPendingPayoutRequestsAsync(ct);
        return Ok(result);
    }

    /// POST api/payments/admin/payout-requests/process
    [HttpPost("admin/payout-requests/process")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProcessPayoutRequest(
        [FromBody] ProcessPayoutRequestDto dto,
        CancellationToken ct)
    {
        await _paymentService.ProcessPayoutRequestAsync(dto, CurrentUserId, ct);
        return Ok(new { message = "Payout request processed successfully" });
    }
}