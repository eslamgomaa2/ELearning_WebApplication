using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Repositories.Payments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Payments;

public class InstructorEarningRepository
    : GenericRepository<InstructorEarning, int>, IInstructorEarningRepository
{
    public InstructorEarningRepository(ELearningDbContext context) : base(context) { }

    public async Task<List<InstructorEarning>> GetInstructorEarningsAsync(
        Guid instructorId, CancellationToken ct = default)
        => (await FindAsync(
            e => e.InstructorId == instructorId,
            include: q => q.Include(e => e.Course),
            orderBy: q => q.OrderByDescending(e => e.CreatedAt),
            ct: ct)).ToList();

    public async Task<decimal> GetAvailableBalanceAsync(
        Guid instructorId, CancellationToken ct = default)
        => await _set
            .Where(e =>
                e.InstructorId == instructorId &&
                e.Status == EarningStatus.Available)
            .SumAsync(e => e.NetAmount, ct);
}