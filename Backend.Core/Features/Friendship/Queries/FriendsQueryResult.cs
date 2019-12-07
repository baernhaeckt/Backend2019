using System;

namespace Backend.Core.Features.Friendship.Queries
{
    public class FriendsQueryResult
    {
        public FriendsQueryResult(Guid id, string displayName, int points, double co2Saving, string email, double? latitude, double? longitude)
        {
            Id = id;
            DisplayName = displayName;
            Points = points;
            Co2Saving = co2Saving;
            Email = email;
            Location = new FriendsQueryResultLocation(latitude ?? 0.0, longitude ?? 0.0);
        }

        public Guid Id { get; }

        public string DisplayName { get; }

        public int Points { get; }

        public double Co2Saving { get; }

        public string Email { get; }

        public FriendsQueryResultLocation Location { get; }
    }
}