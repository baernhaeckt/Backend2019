using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class UserProfileQueryHandler : ISubscriber
    {
        private readonly IReader _reader;

        public UserProfileQueryHandler(IReader reader)
        {
            _reader = reader;
        }

        public async Task<UserProfileQueryResult> ExecuteAsync(UserProfileQuery query)
        {
            return await _reader.SingleAsync<User, UserProfileQueryResult>(
                u => u.Id == query.Id,
                u => new UserProfileQueryResult
                {
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Longitude = u.Location.Longitude,
                    Latitude = u.Location.Latitude,
                    Points = u.Points
                });
        }
    }
}