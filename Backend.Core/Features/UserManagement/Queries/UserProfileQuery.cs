using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Queries
{
    public class UserProfileQuery : IQuery<UserProfileQueryResult>
    {
        public UserProfileQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}