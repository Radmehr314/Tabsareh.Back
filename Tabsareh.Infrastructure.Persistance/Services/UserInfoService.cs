using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tabsareh.Application.Contracts.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tabsareh.Infrastructure.Persistance.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public UserInfoService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public string GetUserIdByToken()
        {
            var tokenStr = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", string.Empty);
            if (string.IsNullOrWhiteSpace(tokenStr)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!);
            tokenHandler.ValidateToken(tokenStr, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
        }

        public string? GetRoleByToken()
        {
            var tokenStr = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", string.Empty);
            if (string.IsNullOrWhiteSpace(tokenStr)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!);
            tokenHandler.ValidateToken(tokenStr, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).FirstOrDefault();
        }
    }
}
