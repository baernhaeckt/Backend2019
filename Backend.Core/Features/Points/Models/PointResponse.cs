using System;
using Backend.Core.Entities;

namespace Backend.Core.Features.Points.Models
{
    public class PointResponse
    {
        public string Text { get; set; } = string.Empty;

        public int Value { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public double Co2Saving { get; set; }

        public UserSufficientType? SufficientType { get; set; }
    }
}