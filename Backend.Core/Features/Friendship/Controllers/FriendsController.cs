using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Extensions;
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
        private readonly FriendsService _friendService;

        private readonly IQueryPublisher _queryPublisher;

        private readonly ICommandPublisher _commandPublisher;

        public FriendsController(FriendsService friendService, IQueryPublisher queryPublisher, ICommandPublisher commandPublisher)
        {
            _friendService = friendService;
            _queryPublisher = queryPublisher;
            _commandPublisher = commandPublisher;
        }

        [HttpGet]
        public async Task<IEnumerable<FriendsQueryResult>> Get() => await _queryPublisher.ExecuteAsync(new FriendsQuery(User.Id()));

        [HttpPost]
        public async Task Add(string friendEmail)
        {
            await _friendService.AddFriend(friendEmail);
        }

        [HttpDelete]
        public async Task Delete(Guid friendUserId)
        {
            await _friendService.RemoveFriend(friendUserId);
        }
    }
}