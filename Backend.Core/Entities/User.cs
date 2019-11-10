﻿using System;
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

        [Obsolete("Calculate using the point history.")]
        public int Points { get; set; }

        [Obsolete("Calculate using the point history.")]
        public double Co2Saving { get; set; }

        public IList<PointAction> PointHistory { get; set; } = Enumerable.Empty<PointAction>().ToList();

        public IList<Award> Awards { get; set; } = Enumerable.Empty<Award>().ToList();

        public List<string> Roles { get; set; } = Enumerable.Empty<string>().ToList();
    }
}