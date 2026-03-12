using E_Learning.core.Interfaces.Repositories.Authentications;
using E_Learning.Core.Entities.Identity;
using E_Learning.Repository.Data;
using E_Learning.Repository.Repositories.GenericesRepositories;

public class OtpCodeRepository : GenericRepository<OtpCode, int>, IOtpCodeRepository
{
    public OtpCodeRepository(ELearningDbContext context) : base(context) { }

    public Task<OtpCode?> GetValidOtpAsync(
        Guid userId, string code, string purpose, CancellationToken ct = default)
        => FirstOrDefaultAsync(o =>
            o.UserId == userId &&
            o.Code == code &&
            o.Purpose == purpose &&
            !o.IsUsed &&
            o.ExpiresAt > DateTime.UtcNow, ct);
}