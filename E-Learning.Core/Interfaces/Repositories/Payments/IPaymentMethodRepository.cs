using E_Learning.Core.Entities.Billing;

namespace E_Learning.Core.Interfaces.Repositories.Payments;

public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod, int>
{
    Task<List<PaymentMethod>> GetUserPaymentMethodsAsync(
        Guid userId, CancellationToken ct = default);

    Task<PaymentMethod?> GetDefaultAsync(
        Guid userId, CancellationToken ct = default);

    Task ClearDefaultAsync(
        Guid userId, CancellationToken ct = default);
}