using E_Learning.Core.Entities.Identity;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Authentications;

public interface IUserSessionRepository : IGenericRepository<UserSession, int>
{
    Task<UserSession?> GetActiveSessionByTokenAsync(
        string refreshToken,
        CancellationToken ct = default);

    Task RevokeAllUserSessionsAsync(
        Guid userId,
        CancellationToken ct = default);
}