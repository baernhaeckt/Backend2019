using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Backend.Database
{
    public class User : IMongoEntity
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public Location Location { get; set; }

        public string Password { get; set; }

        public IEnumerable<Guid> Friends { get; set; }

        public int Points { get; set; }

        public IEnumerable<PointAction> PointActions { get; set; }
    }
}
