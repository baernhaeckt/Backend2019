using System;

namespace Backend.Core.Features.Points.Queries
{
    public class RankingQueryResult
    {
        public RankingQueryResult(Guid id, string displayName, int points)
        {
            Id = id;
            DisplayName = displayName;
            Points = points;
        }

        public Guid Id { get; }

        public string DisplayName { get; }

        public int Points { get; }
    }
}