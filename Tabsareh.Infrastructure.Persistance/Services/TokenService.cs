using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tabsareh.Application.Contracts.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tabsareh.Infrastructure.Persistance.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public AccessToken Generate(string userId, long tokenVersion, string deviceId, string role = "user", IEnumerable<string>? permissions = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var exp = now.AddMinutes(400);

            var claims = new List<Claim>
            {
                new Claim("sub", userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("tver", tokenVersion.ToString()),
                new Claim("did", deviceId),
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim("role", role),
                new Claim(ClaimTypes.Role, role)
            };

            if (permissions != null)
                foreach (var perm in permissions)
                    claims.Add(new Claim("permission", perm));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims, now, exp, creds);
            var value = new JwtSecurityTokenHandler().WriteToken(token);
            return new AccessToken(value, exp);
        }

        public string GenerateAccessToken(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
