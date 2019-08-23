using AspNetCore.MongoDB;

namespace Backend.Models.Database
{
    public class User : IMongoEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
