using System;

namespace Backend.Core.Entities
{
    public class SufficientType : Entity, ICloneable
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int BaselinePoint { get; set; }

        public double BaselineCo2Saving { get; set; }

        public object Clone() => MemberwiseClone();
    }
}