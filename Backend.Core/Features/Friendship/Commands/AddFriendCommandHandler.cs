using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Friendship.Commands
{
    public class AddFriendCommandHandler : CommandHandler<AddFriendCommand>
    {
        public AddFriendCommandHandler(IUnitOfWork unitOfWork, ILogger<AddFriendCommandHandler> logger)
            : base(unitOfWork, logger)
        {
        }

        public override async Task ExecuteAsync(AddFriendCommand command)
        {
            Logger.ExecuteAddFriend(command.UserId, command.FriendEmail);

            FriendUser? userToBecomeFriendWith = await UnitOfWork.SingleOrDefaultAsync<User, FriendUser>(
                u => u.Email == command.FriendEmail.ToLowerInvariant() && u.Roles.Any(r => r == Roles.User),
                u => new FriendUser(u.Id, u.Friends));

            if (userToBecomeFriendWith == null)
            {
                throw new ValidationException("No user with this e-mail address found.");
            }

            if (command.UserId == userToBecomeFriendWith.Id)
            {
                throw new ValidationException("You must not be your own friend.");
            }

            FriendUser currentUser = await UnitOfWork.GetByIdOrThrowAsync<User, FriendUser>(command.UserId, u => new FriendUser(u.Id, u.Friends));
            if (currentUser.Friends.Contains(userToBecomeFriendWith.Id) || userToBecomeFriendWith.Friends.Contains(command.UserId))
            {
                throw new ValidationException("This user is already your friend.");
            }

            // Its necessary to update friends on both users.
            // Actually a transaction is necessary here, currently the naive approach is taken.
            // In the future, e.g. consider to use a appropriate data structure  (own collection) or introduce mongodb transactions.
            await UnitOfWork.UpdateAsync<User>(command.UserId, new { Friends = new List<Guid> { userToBecomeFriendWith.Id } });
            await UnitOfWork.UpdateAsync<User>(userToBecomeFriendWith.Id, new { Friends = new List<Guid> { command.UserId } });

            Logger.ExecuteAddFriendSuccessful(command.UserId, command.FriendEmail, userToBecomeFriendWith.Id);
        }

        private class FriendUser
        {
            public FriendUser(Guid id, IEnumerable<Guid> friends)
            {
                Id = id;
                Friends = friends;
            }

            public Guid Id { get; }

            public IEnumerable<Guid> Friends { get; }
        }
    }
}