using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Interfaces.Repositories.Payments;
using E_Learning.Repository.Data;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Payments;

public class PaymentMethodRepository
    : GenericRepository<PaymentMethod, int>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ELearningDbContext context) : base(context) { }

    public async Task<List<PaymentMethod>> GetUserPaymentMethodsAsync(
        Guid userId, CancellationToken ct = default)
        => (await FindAsync(pm => pm.UserId == userId, ct: ct)).ToList();

    public async Task<PaymentMethod?> GetDefaultAsync(
        Guid userId, CancellationToken ct = default)
        => await FirstOrDefaultAsync(
            pm => pm.UserId == userId && pm.IsDefault, ct);

    public async Task ClearDefaultAsync(
        Guid userId, CancellationToken ct = default)
    {
        var defaults = await FindAsync(
            pm => pm.UserId == userId && pm.IsDefault, ct: ct);

        foreach (var pm in defaults)
        {
            pm.IsDefault = false;
            Update(pm);
        }
    }
}