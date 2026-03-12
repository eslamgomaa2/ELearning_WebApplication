using E_Learning.core.Interfaces.Repositories.Authentications;
using E_Learning.Core.Entities.Identity;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;

public class UserSessionRepository : GenericRepository<UserSession, int>, IUserSessionRepository
{
    public UserSessionRepository(ELearningDbContext context) : base(context) { }

    public Task<UserSession?> GetActiveSessionByTokenAsync(
        string refreshToken, CancellationToken ct = default)
        => FirstOrDefaultAsync(s =>
            s.RefreshToken == refreshToken &&
            s.IsActive &&
            s.ExpiresAt > DateTime.UtcNow, ct);

    public async Task RevokeAllUserSessionsAsync(
        Guid userId, CancellationToken ct = default)
    {
        var sessions = await FindAsync(
            s => s.UserId == userId && s.IsActive, ct: ct);

        foreach (var s in sessions)
        {
            s.IsActive = false;
            Update(s);
        }
    }
}