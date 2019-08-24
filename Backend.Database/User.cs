using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;

namespace Backend.Database
{
    public class User : IMongoEntity
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public Location Location { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Friends { get; set; }

        public int Points { get; set; }

        public IEnumerable<PointAction> PointActions { get; set; }
    }
}
