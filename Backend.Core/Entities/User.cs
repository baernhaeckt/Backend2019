using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Core.Entities.Awards;

namespace Backend.Core.Entities
{
    public class User : Entity
    {
        public string Email { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public Location Location { get; set; } = new Location();

        public string PasswordHash { get; set; } = string.Empty;

        public IList<Guid> Friends { get; set; } = Enumerable.Empty<Guid>().ToList();

        /// <summary>
        ///     Gets or sets the total points earned by this user.
        ///     <remarks>
        ///         This can also be calculated using the PointHistory, should always be the same as the sum of all points in the
        ///         history.
        ///     </remarks>
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        ///     Gets or sets the total amount of saved CO2 by this user.
        ///     <remarks>
        ///         This can also be calculated using the PointHistory, should always be the same as the sum of all points in the
        ///         history.
        ///     </remarks>
        /// </summary>
        public double Co2Saving { get; set; }

        public IList<PointAction> PointHistory { get; set; } = Enumerable.Empty<PointAction>().ToList();

        public IList<Award> Awards { get; set; } = Enumerable.Empty<Award>().ToList();

        public List<string> Roles { get; set; } = Enumerable.Empty<string>().ToList();
    }
}