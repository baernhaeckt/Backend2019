﻿using Backend.Database;
using System;

namespace Backend.Models
{
    public class PointResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public double Co2Saving { get; set; }

        public UserSufficientType SufficientType { get; set; }
    }
}
