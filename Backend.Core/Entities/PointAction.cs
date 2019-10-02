﻿using System;

namespace Backend.Core.Entities
{
    public class PointAction
    {
        public int Point { get; set; }

        public double Co2Saving { get; set; }

        public string Action { get; set; } = string.Empty;

        public string SponsorRef { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public UserSufficientType SufficientType { get; set; } = new UserSufficientType();
    }
}