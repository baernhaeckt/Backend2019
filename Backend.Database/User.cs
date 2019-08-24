using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IList<Award> Awards { get; set; } = Enumerable.Empty<Award>().ToList();
    }
}
