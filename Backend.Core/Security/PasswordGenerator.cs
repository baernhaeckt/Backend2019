using Backend.Core.Security.Abstraction;

namespace Backend.Core.Security
{

    public class StaticPasswordGenerator : IPaswordGenerator
    {
        public string Generate() => "1234";
    }
}
