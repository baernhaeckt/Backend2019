using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class UserProfileQueryHandler : QueryHandler<UserProfileQueryResult, UserProfileQuery>
    {
        public UserProfileQueryHandler(IReader reader, ILogger<UserProfileQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<UserProfileQueryResult> ExecuteAsync(UserProfileQuery query)
        {
            Logger.RetrieveUserProfile(query.Id);

            UserProfileQueryResult result = await Reader.SingleAsync<User, UserProfileQueryResult>(
                u => u.Id == query.Id,
                u => new UserProfileQueryResult(u.DisplayName, u.Points, u.Email, u.Location.Latitude, u.Location.Longitude, u.Location.City, u.Location.Street, u.Location.PostalCode));

            Logger.RetrieveUserProfileSuccessful(query.Id);

            return result;
        }
    }
}