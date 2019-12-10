using System;
using Backend.Core.Entities;

namespace Backend.Core.Features.Points.Queries
{
    public class PointHistoryForUserQueryResult
    {
        public PointHistoryForUserQueryResult(DateTimeOffset date, string text, int points, double co2Saving, SufficientType type)
        {
            Date = date;
            Text = text;
            Points = points;
            Co2Saving = co2Saving;
            Type = type;
        }

        public string Text { get; }

        public int Points { get; }

        public DateTimeOffset Date { get; }

        public double Co2Saving { get; }

        public SufficientType Type { get; }
    }
}