using System.Security.Claims;

namespace Hannet.WebApi.Infrastructure.Extentsions
{
    public static class IdentityExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
            => user
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
    }
}