using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
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
        public async Task<IEnumerable<PrivateUserResponse>> GetAsync()
        {
            IEnumerable<User> result = await FriendService.GetFriends();
            return result.Select(u => new PrivateUserResponse()
            {
                Id = u.Id,
                Email = u.Email,
                DisplayName = u.DisplayName,
                Location = new LocationResponse
                {
                    Longitude = u.Location?.Longitude ?? 0.0,
                    Latitude = u.Location?.Latitude ?? 0.0
                },
                Points = u.Points
            });
        }

        [HttpPost]
        public async Task Create(string friendEmail) => await FriendService.AddFriend(friendEmail);

        [HttpDelete]
        public async Task Delete(Guid friendUserId) => await FriendService.RemoveFriend(friendUserId);
    }
}