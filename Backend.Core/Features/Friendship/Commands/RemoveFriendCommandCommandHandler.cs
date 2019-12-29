using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Friendship.Commands
{
    public class RemoveFriendCommandCommandHandler : CommandHandler<RemoveFriendCommand>
    {
        public RemoveFriendCommandCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveFriendCommandCommandHandler> logger)
            : base(unitOfWork, logger)
        {
        }

        public override async Task ExecuteAsync(RemoveFriendCommand command)
        {
            Logger.ExecuteRemoveFriend(command.UserId, command.FriendUserId);

            IEnumerable<Guid>? friends = await UnitOfWork.GetByIdOrThrowAsync<User, IEnumerable<Guid>>(command.UserId, u => u.Friends);
            if (!friends.Contains(command.FriendUserId))
            {
                throw new ValidationException("This user is not your friend.");
            }

            await UnitOfWork.UpdatePullAsync<User, Guid>(command.UserId, u => u.Friends, command.FriendUserId);
            await UnitOfWork.UpdatePullAsync<User, Guid>(command.FriendUserId, u => u.Friends, command.UserId);

            Logger.RemoveFriendSuccessful(command.UserId, command.FriendUserId);
        }
    }
}