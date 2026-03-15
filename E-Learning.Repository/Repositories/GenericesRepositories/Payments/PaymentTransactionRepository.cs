using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Interfaces.Repositories.Payments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Payments;

public class PaymentTransactionRepository
    : GenericRepository<PaymentTransaction, int>, IPaymentTransactionRepository
{
    public PaymentTransactionRepository(ELearningDbContext context) : base(context) { }

    public async Task<PaymentTransaction?> GetByGatewayReferenceAsync(
        string gatewayReference, CancellationToken ct = default)
        => await FirstOrDefaultAsync(
            t => t.GatewayReference == gatewayReference, ct);

    public async Task<List<PaymentTransaction>> GetStudentTransactionsAsync(
        Guid studentId, CancellationToken ct = default)
        => (await FindAsync(
            t => t.StudentId == studentId,
            include: q => q.Include(t => t.Course),
            orderBy: q => q.OrderByDescending(t => t.CreatedAt),
            ct: ct)).ToList();
}