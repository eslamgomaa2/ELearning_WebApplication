using E_Learning.Core.Entities.Identity;

namespace E_Learning.Core.Specifications.Auth;

public class ActiveUserSessionsSpecification : BaseSpecification<UserSession>
{
    public ActiveUserSessionsSpecification(Guid userId)
        : base(s => s.UserId == userId && s.IsActive)
    {
    }
}