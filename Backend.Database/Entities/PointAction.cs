
using System;
using System.Collections.Generic;

namespace Backend.Database
{
    public class PointAction : Entity
    {
        public int Point { get; set; }

        public double Co2Saving { get; set; }

        public string Action { get; set; }

        public IEnumerable<MetaData> MetaData { get; set; }

        public String SponsorRef { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public UserSufficientType SufficientType { get; set; }
    }
}
