namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using System.Security.Claims;

    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string userName, IEnumerable<Claim> additionalClaims = null);
    }
}
