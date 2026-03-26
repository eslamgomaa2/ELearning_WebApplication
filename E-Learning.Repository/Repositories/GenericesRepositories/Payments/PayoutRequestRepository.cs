using E_Learning.Core.Entities.Billing;
using E_Learning.Core.Enums;
using E_Learning.Core.Interfaces.Repositories.Payments;
using E_Learning.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repository.Repositories.GenericesRepositories.Payments;

public class PayoutRequestRepository
    : GenericRepository<PayoutRequest, int>, IPayoutRequestRepository
{
    public PayoutRequestRepository(ELearningDbContext context) : base(context) { }

    public async Task<List<PayoutRequest>> GetInstructorPayoutsAsync(
        Guid instructorId, CancellationToken ct = default)
        => (await FindAsync(
            pr => pr.InstructorId == instructorId,
            orderBy: q => q.OrderByDescending(pr => pr.RequestedAt),
            ct: ct)).ToList();

    public async Task<List<PayoutRequest>> GetPendingPayoutsAsync(
        CancellationToken ct = default)
        => (await FindAsync(
            pr => pr.Status == PayoutStatus.Pending,
            include: q => q.Include(pr => pr.Instructor),
            orderBy: q => q.OrderBy(pr => pr.RequestedAt),
            ct: ct)).ToList();

    public async Task<bool> HasPendingRequestAsync(
        Guid instructorId, CancellationToken ct = default)
        => await AnyAsync(
            pr => pr.InstructorId == instructorId &&
                  pr.Status == PayoutStatus.Pending, ct);
}