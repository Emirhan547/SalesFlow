using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalesFlow.Business.Services.JwtServices
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }
        public string CreateAccessToken(TokenUser user)
        {
            DateTime expireDate = GetAccessTokenExpireDate();

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (string role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey securityKey =
                new(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            SigningCredentials credentials =
                new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expireDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            byte[] randomNumber = new byte[64];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public DateTime GetAccessTokenExpireDate()
            => DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        public DateTime GetRefreshTokenExpireDate()
            => DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
    }
}

