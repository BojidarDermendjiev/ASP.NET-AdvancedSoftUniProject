namespace ServerAspNetCoreAPIMakePC.Infrastructure.Services
{
    using System.Text;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;

    using Application.Interfaces;

    public class JwtTokenService : IJwtTokenService
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtLifespanMinutes;

        public JwtTokenService(IConfiguration configuration)
        {
            this._jwtSecret = configuration["Jwt:Secret"];
            this._jwtIssuer = configuration["Jwt:Issuer"];
            this._jwtAudience = configuration["Jwt:Audience"];
            this._jwtLifespanMinutes = int.TryParse(configuration["Jwt:LifespanMinutes"], out var lifespan) ? lifespan : 60;
        }
        public string GenerateToken(string userId, string userName, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: this._jwtIssuer,
                audience: this._jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(this._jwtLifespanMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
