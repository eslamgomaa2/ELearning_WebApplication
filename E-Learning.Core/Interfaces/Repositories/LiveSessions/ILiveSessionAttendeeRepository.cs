using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;

namespace E_Learning.Core.Interfaces.Repositories.LiveSessions

{
    public interface ILiveSessionAttendeeRepository
    {
       Task AddAsync(LiveSessionAttendee attendee, CancellationToken ct = default);

        Task<bool> IsStudentEnrolledAsync(int sessionId, Guid studentId, CancellationToken ct = default);

        Task<IReadOnlyList<LiveSessionAttendee>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default);    }
}