using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FriendsService _friendsService;

        public RankingsController(UserService userService, FriendsService friendsService)
        {
            _userService = userService;
            _friendsService = friendsService;
        }

        [HttpGet("global")]
        public async Task<IEnumerable<UserResponse>> GetGlobalAsync()
        {
            var users = await _userService.GetAllAsync();
            var results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("local")]
        public async Task<IEnumerable<UserResponse>> GetLocalAsync(string zip)
        {
            IEnumerable<User> users = await _userService.GetByPlzAsync(zip);

            var results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("friends")]
        public async Task<IEnumerable<UserResponse>> GetFriendsAsync()
        {
            var results = CreateResult((await _friendsService.GetFriends()));

            return results.OrderByDescending(u => u.Points);
        }

        [HttpGet("summary")]
        public async Task<RankingSummary> GetSummary()
        {
            var currentUser = _userService.CurrentUser;
            var zipCode = currentUser.Location?.Zip ?? "3000";

            var allUsers = await _userService.GetAllAsync();
            var global = CreateResult(allUsers).OrderByDescending(u => u.Points);
            var local = CreateResult(allUsers.Where(u => u.Location != null && u.Location.Zip == zipCode)).OrderByDescending(u => u.Points);
            var friends = CreateResult(await _friendsService.GetFriends()).OrderByDescending(u => u.Points);

            return new RankingSummary
            {
                FriendRank = GetRank(friends, currentUser),
                GlobalRank = GetRank(global, currentUser),
                LocalRank = GetRank(local, currentUser)
            };
        }

        private static int GetRank(IEnumerable<UserResponse> list, User user)
        {
            var rank = 1;
            foreach (var u in list)
            {
                if (u.Id == user.Id)
                {
                    break;
                }
                rank++;
            }

            return rank;
        }

        private IEnumerable<UserResponse> CreateResult(IEnumerable<User> users)
        {
            users = users.ToList();
            IList<UserResponse> results = new List<UserResponse>(users.Count());

            foreach (var user in users)
            {
                int pointsForUser = user.PointActions.Sum(p => p.Point);

                results.Add(new UserResponse
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Points = pointsForUser
                });
            }

            return results;
        }
    }
}