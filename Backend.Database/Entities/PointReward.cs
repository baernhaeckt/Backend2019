
using System.Collections.Generic;

namespace Backend.Database
{
    public class PointReward : Entity
    {
        public int Value { get; set; }

        public string Text { get; set; }

        public IEnumerable<MetaData> MetaData { get; set; }
    }
}
