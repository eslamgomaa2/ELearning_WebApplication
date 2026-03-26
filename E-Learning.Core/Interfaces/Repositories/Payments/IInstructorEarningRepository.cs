using E_Learning.Core.Entities.Billing;

namespace E_Learning.Core.Interfaces.Repositories.Payments;

public interface IInstructorEarningRepository : IGenericRepository<InstructorEarning, int>
{
    Task<List<InstructorEarning>> GetInstructorEarningsAsync(
        Guid instructorId, CancellationToken ct = default);

    Task<decimal> GetAvailableBalanceAsync(
        Guid instructorId, CancellationToken ct = default);
}