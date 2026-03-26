using E_Learning.Core.Entities.Billing;

namespace E_Learning.Core.Interfaces.Repositories.Payments;

public interface IPaymentTransactionRepository : IGenericRepository<PaymentTransaction, int>
{
    Task<PaymentTransaction?> GetByGatewayReferenceAsync(
        string gatewayReference, CancellationToken ct = default);

    Task<List<PaymentTransaction>> GetStudentTransactionsAsync(
        Guid studentId, CancellationToken ct = default);
}