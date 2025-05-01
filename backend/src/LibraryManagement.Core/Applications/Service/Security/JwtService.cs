using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Core.Application.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Core.Applications.Service.Security
{
    public class JwtService
    {
        private readonly byte[] _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenValidityMins;

        public JwtService(IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtConfig");
            _key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
            _issuer = jwtSection["Issuer"]!;
            _audience = jwtSection["Audience"]!;
            _tokenValidityMins = configuration.GetValue<int>("JwtConfig:TokenValidityMins");
        }

        public UserResponse GenerateToken(string userName, UserType userType)
        {
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(_tokenValidityMins);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(ClaimTypes.Role, userType.ToString()),
            };

            var tokenDecreptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryTimeStamp,
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDecreptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new UserResponse
            {
                Name = userName,
                AccessToken = accessToken,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
