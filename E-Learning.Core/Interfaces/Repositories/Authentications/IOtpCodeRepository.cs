using E_Learning.Core.Entities.Identity;
using E_Learning.Core.Interfaces.Repositories;

namespace E_Learning.core.Interfaces.Repositories.Authentications;

public interface IOtpCodeRepository : IGenericRepository<OtpCode, int>
{
    Task<OtpCode?> GetValidOtpAsync(
        Guid userId,
        string code,
        string purpose,
        CancellationToken ct = default);
}