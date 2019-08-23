using AspNetCore.MongoDB;

namespace Backend.Models
{
    public class LoginResponse : IMongoEntity
    {
        public string Token { get; set; }
    }
}
