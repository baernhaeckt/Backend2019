using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Models
{
    public class PointAwarding
    {
        public PointAwardingKind Source { get; set; }

        public int Points { get; set; }

        public string Text { get; set; }

        public double Co2Saving { get; set; }
    }
}
