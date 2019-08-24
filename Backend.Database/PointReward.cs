using AspNetCore.MongoDB;
using System.Collections.Generic;

namespace Backend.Database
{
    public class PointReward : IMongoEntity
    {
        public int Value { get; set; }

        public string Text { get; set; }

        public IEnumerable<MetaData> MetaData { get; set; }
    }
}
