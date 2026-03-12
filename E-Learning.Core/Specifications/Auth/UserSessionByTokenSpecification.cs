using E_Learning.Core.Entities.Identity;

namespace E_Learning.Core.Specifications.Auth;

public class UserSessionByTokenSpecification : BaseSpecification<UserSession>
{
    public UserSessionByTokenSpecification(string refreshToken)
        : base(s =>
            s.RefreshToken == refreshToken &&
            s.IsActive &&
            s.ExpiresAt > DateTime.UtcNow)
    {
    }
}