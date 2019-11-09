using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Configuration;

namespace Backend.Infrastructure.Security
{
    public class StaticPasswordGenerator : IPasswordGenerator
    {
        private readonly IConfiguration _configuration;

        public StaticPasswordGenerator(IConfiguration configuration) => _configuration = configuration;

        public string Generate() => _configuration["DefaultPassword"];
    }
}