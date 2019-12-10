using Backend.Core.Entities;

namespace Backend.Core.Features.Baseline.Queries
{
    public class SufficientTypesQueryResult
    {
        public SufficientTypesQueryResult(SufficientType type, double points, double co2Savings)
        {
            Type = type;
            Points = points;
            Co2Savings = co2Savings;
        }

        public double Points { get; }

        public SufficientType Type { get; }

        public double Co2Savings { get; }
    }
}