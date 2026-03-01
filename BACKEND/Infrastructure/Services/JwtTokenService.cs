using Application.Interfaces.Common;
using Application.Shared.Time;
using Domain.User;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IDateTimeProvider _dateTimeProvider;

        public JwtTokenService(
            IOptions<JwtOptions> jwtOptions,
            IDateTimeProvider dateTimeProvider)
        {
            _jwtOptions = jwtOptions.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public string GenerateAccessToken(User user, string roleName)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.EmailAddress),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var expires = _dateTimeProvider.UtcNow
                .AddMinutes(_jwtOptions.ExpiryMinutes)
                .UtcDateTime;

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
