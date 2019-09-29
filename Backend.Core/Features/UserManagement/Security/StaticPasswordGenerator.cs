using Backend.Core.Features.UserManagement.Security.Abstraction;
using Microsoft.Extensions.Configuration;

namespace Backend.Core.Features.UserManagement.Security
{
    public class StaticPasswordGenerator : IPasswordGenerator
    {
        private readonly IConfiguration _configuration;

        public StaticPasswordGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate()
        {
            return _configuration["DefaultPassword"];
        }
    }
}