using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Services;

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
        public IEnumerable<PrivateUserResponse> Get()
        {
            return FriendService.Friends.Select(u => new PrivateUserResponse()
            {
                Id = u.Id,
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
        public async Task Create(string friendEmail)
        {
            await FriendService.AddFriend(friendEmail);
        }

        [HttpDelete]
        public async Task Delete(string friendUserId)
        {
            await FriendService.RemoveFriend(friendUserId);
        }
    }
}