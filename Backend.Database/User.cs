using AspNetCore.MongoDB;
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

        public double Co2Saving { get; set; }

        public IList<PointAction> PointActions { get; set; } = Enumerable.Empty<PointAction>().ToList();

        public IList<Award> Awards { get; set; } = Enumerable.Empty<Award>().ToList();
    }
}
