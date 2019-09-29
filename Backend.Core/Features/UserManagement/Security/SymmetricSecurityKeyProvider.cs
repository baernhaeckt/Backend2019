using System.Text;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Features.UserManagement.Security
{
    public class SymmetricSecurityKeyProvider : ISecurityKeyProvider
    {
        private readonly IConfiguration _configuration;

        public SymmetricSecurityKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
        }
    }
}