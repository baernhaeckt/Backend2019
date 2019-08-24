using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;

namespace Backend.Database
{
    public class PointAction : IMongoEntity
    {
        public int Point { get; set; }

        public string Action { get; set; }

        public IEnumerable<MetaData> MetaData { get; set; }

        public String SponsorRef { get; set; }

        DateTime Date { get; set; } = DateTime.Now;
    }
}
