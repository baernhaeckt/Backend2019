
using System;

namespace Backend.Database
{
    public class Token : Entity
    {
        public string Partner { get; set; }

        public string Text { get; set; }

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public Guid UserId { get; set; }

        public bool Valid => UserId == Guid.Empty;

        public Guid Value { get; set; }

        public SufficientType SufficientType { get; set; }
    }
}
