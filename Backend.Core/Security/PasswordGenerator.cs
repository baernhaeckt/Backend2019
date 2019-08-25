using Backend.Core.Security.Abstraction;
using Microsoft.Extensions.Configuration;

namespace Backend.Core.Security
{
    public class StaticPasswordGenerator : IPaswordGenerator
    {
        private readonly IConfiguration _configuration;

        public StaticPasswordGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Generate() => _configuration["DefaultPassword"];
    }
}
