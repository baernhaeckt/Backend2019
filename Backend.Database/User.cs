using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;

namespace Backend.Models.Database
{
    public class User : IMongoEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<Guid> Friends { get; set; }
    }
}
