using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Backend.Infrastructure.Security.Abstraction;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Backend.Infrastructure.Security
{
    public class JwtSecurityTokenFactory : ISecurityTokenFactory
    {
        private readonly ISecurityKeyProvider _securityKeyProvider;

        public JwtSecurityTokenFactory(ISecurityKeyProvider securityKeyProvider) => _securityKeyProvider = securityKeyProvider;

        public string Create(Guid id, string subject, IEnumerable<string> roles)
        {
            DateTime now = DateTime.Now;
            var claims = new List<Claim>
            {
                new Claim(LeafClaimTypes.UserId, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, subject),
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.CurrentCulture), ClaimValueTypes.Integer64)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            SymmetricSecurityKey signingKey = _securityKeyProvider.GetSecurityKey();
            var securityToken = new JwtSecurityToken(
                "Leaf",
                "Leaf",
                claims,
                now,
                now.AddMinutes(20),
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}