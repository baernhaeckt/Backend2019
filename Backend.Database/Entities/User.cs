
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Database
{
    public class User : Entity
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public Location Location { get; set; }

        public string Password { get; set; }

        public IList<Guid> Friends { get; set; } = Enumerable.Empty<Guid>().ToList();

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public IList<PointAction> PointActions { get; set; } = Enumerable.Empty<PointAction>().ToList();

        public IList<Award> Awards { get; set; } = Enumerable.Empty<Award>().ToList();

        public List<string> Roles { get; set; } = Enumerable.Empty<string>().ToList();
    }
}
