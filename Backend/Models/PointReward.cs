using System.Collections.Generic;

namespace Backend.Models
{
    public class PointReward
    {
        public int Value { get; set; }

        public string Text { get; set; }

        public IEnumerable<MetaData> MetaData { get; set; }
    }
}
