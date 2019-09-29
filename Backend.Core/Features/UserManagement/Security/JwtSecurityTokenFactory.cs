using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Backend.Database;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Features.UserManagement.Security
{
    public class JwtSecurityTokenFactory : ISecurityTokenFactory
    {
        private readonly ISecurityKeyProvider _securityKeyProvider;

        public JwtSecurityTokenFactory(ISecurityKeyProvider securityKeyProvider)
        {
            _securityKeyProvider = securityKeyProvider;
        }

        public string Create(User user)
        {
            DateTime now = DateTime.Now;
            var claims = new List<Claim>
            {
                new Claim(LeafClaimTypes.UserId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.CurrentCulture), ClaimValueTypes.Integer64),
            };

            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            SymmetricSecurityKey signingKey = _securityKeyProvider.GetSecurityKey();
            var securityToken = new JwtSecurityToken(
                issuer: "Leaf",
                audience: "Leaf",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(20),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
