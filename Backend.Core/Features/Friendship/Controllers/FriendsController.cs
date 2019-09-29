using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Backend.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Friendship.Controllers
{
    [Authorize]
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly FriendsService _friendService;

        public FriendsController(FriendsService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<IEnumerable<PrivateUserResponse>> GetAsync()
        {
            IEnumerable<User> result = await _friendService.GetFriends();
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
        public async Task Create(string friendEmail) => await _friendService.AddFriend(friendEmail);

        [HttpDelete]
        public async Task Delete(Guid friendUserId) => await _friendService.RemoveFriend(friendUserId);
    }
}