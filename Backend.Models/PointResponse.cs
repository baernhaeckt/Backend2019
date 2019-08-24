using Backend.Database;
using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class PointResponse
    {
        public string Id { get; set; }

        public String Text { get; set; }

        public int Value { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public double Co2Saving { get; set; }

        public UserSufficientType SufficientType { get; set; }
    }
}
