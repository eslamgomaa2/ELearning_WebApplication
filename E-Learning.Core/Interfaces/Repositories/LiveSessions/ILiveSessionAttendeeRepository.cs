using E_Learning.Core.Base;
using E_Learning.Core.Entities.LiveSessions;
using E_Learning.Core.Entities.Profiles;

namespace E_Learning.Core.Interfaces.Repositories.LiveSessions

{
  public interface ILiveSessionAttendeeRepository
  {
    Task AddAsync(LiveSessionAttendee attendee, CancellationToken ct = default);

    Task<bool> IsStudentEnrolledAsync(int sessionId, Guid studentId, CancellationToken ct = default);

    Task<IReadOnlyList<LiveSessionAttendee>> GetAttendeesBySessionIdAsync(int sessionId, CancellationToken ct = default);
    public IQueryable<StudentProfile> GetTableNoTracking();
    void LeaveSession(LiveSessionAttendee liveSessionAttendee);
        void Update(LiveSessionAttendee liveSessionAttendee);

    public  Task<LiveSessionAttendee?> GetActiveAttendeeAsync(int sessionId, Guid studentId, CancellationToken ct = default);


  }
}