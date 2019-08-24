using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Models.Database;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Security
{
    public class JwtSecurityTokenFactory : ISecurityTokenFactory
    {
        public string Create(User user)
        {
            DateTime now = DateTime.Now;
            List<Claim> claims = new List<Claim>
            {
                new Claim(OekoBookClaimTypes.UserId, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.CurrentCulture), ClaimValueTypes.Integer64),
            };

            SymmetricSecurityKey signingKey = SecurityKeyProvider.GetSecurityKey();
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: "OekoBook",
                audience: "OekoBook",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(20),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
