using System;

namespace Backend.Database.Entities
{
    public class Token : Entity
    {
        public string Partner { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public Guid UserId { get; set; }

        public bool Valid => UserId == Guid.Empty;

        public Guid Value { get; set; }

        public SufficientType SufficientType { get; set; } = new SufficientType();
    }
}