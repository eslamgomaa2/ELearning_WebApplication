using System.Security.Claims;

namespace E_Learning.API.Extensions
{
   

namespace E_Learning.API.Extensions
    {
        public static class ClaimsPrincipalExtensions
        {
            public static Guid GetUserId(this ClaimsPrincipal user)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User ID not found in token");

                return Guid.Parse(userId);
            }
        }
    }
}

