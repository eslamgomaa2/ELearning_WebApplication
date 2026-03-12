using E_Learning.Core.Entities.Identity;

namespace E_Learning.Core.Specifications.Auth;

public class OtpCodeSpecification : BaseSpecification<OtpCode>
{
    public OtpCodeSpecification(Guid userId, string code, string purpose)
        : base(o =>
            o.UserId == userId &&
            o.Code == code &&
            o.Purpose == purpose &&
            !o.IsUsed &&
            o.ExpiresAt > DateTime.UtcNow)
    {
    }
}