using System;
using Backend.Core.Entities;

namespace Backend.Core.Features.Points.Queries
{
    public class PointHistoryForUserQueryResult
    {
        public PointHistoryForUserQueryResult(DateTimeOffset date, string text, int value, double co2Saving, UserSufficientType sufficientType)
        {
            Date = date;
            Text = text;
            Value = value;
            Co2Saving = co2Saving;
            SufficientType = sufficientType;
        }

        public string Text { get; set; }

        public int Value { get; set; }

        public DateTimeOffset Date { get; set; }

        public double Co2Saving { get; set; }

        public UserSufficientType? SufficientType { get; set; }
    }
}