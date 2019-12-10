using System;

namespace Backend.Core.Entities
{
    public class PointAction
    {
        public int Point { get; set; }

        public double Co2Saving { get; set; }

        public string Action { get; set; } = string.Empty;

        public Guid? TokenId { get; set; }

        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;

        public SufficientType Type { get; set; }
    }
}