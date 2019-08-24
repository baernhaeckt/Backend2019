using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        public FriendsService FriendService { get; }

        public FriendsController(FriendsService friendService)
        {
            FriendService = friendService;
        }

        [HttpGet]
        public IEnumerable<PrivateUserResponse> Get()
        {
            return FriendService.Friends.Select(u => new PrivateUserResponse()
            {
                Id = new Guid(u.Id),
                Email = u.Email,
                DisplayName = u.DisplayName,
                Location = new LocationResponse
                {
                    Longitude = u.Location.Longitude,
                    Latitude = u.Location.Latitude
                },
                Points = u.Points
            });
        }

        [HttpPost]
        public async Task Create(Guid friendGuid)
        {
            await FriendService.AddFriend(friendGuid);
        }

        [HttpDelete]
        public async Task Delete(Guid friendGuid)
        {
            await FriendService.RemoveFriend(friendGuid);
        }
    }
}