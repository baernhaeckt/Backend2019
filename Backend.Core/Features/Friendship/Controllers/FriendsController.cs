using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Friendship.Commands;
using Backend.Core.Features.Friendship.Models;
using Backend.Core.Features.Friendship.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Friendship.Controllers
{
    [Authorize]
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        private readonly IQueryPublisher _queryPublisher;

        public FriendsController(IQueryPublisher queryPublisher, ICommandPublisher commandPublisher)
        {
            _queryPublisher = queryPublisher;
            _commandPublisher = commandPublisher;
        }

        [HttpGet]
        public async Task<IEnumerable<FriendsQueryResult>> Get() => await _queryPublisher.ExecuteAsync(new FriendsQuery(User.Id()));

        [HttpPost]
        public async Task Add([FromBody] AddFriendRequest request) => await _commandPublisher.ExecuteAsync(new AddFriendCommand(User.Id(), request.Email));

        [HttpDelete]
        public async Task Delete([FromQuery] Guid friendUserId) => await _commandPublisher.ExecuteAsync(new RemoveFriendCommand(User.Id(), friendUserId));
    }
}