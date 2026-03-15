using E_Learning.Core.Entities.Billing;

namespace E_Learning.Core.Interfaces.Repositories.Payments;

public interface IPayoutRequestRepository : IGenericRepository<PayoutRequest, int>
{
    Task<List<PayoutRequest>> GetInstructorPayoutsAsync(
        Guid instructorId, CancellationToken ct = default);

    Task<List<PayoutRequest>> GetPendingPayoutsAsync(
        CancellationToken ct = default);

    Task<bool> HasPendingRequestAsync(
        Guid instructorId, CancellationToken ct = default);
}