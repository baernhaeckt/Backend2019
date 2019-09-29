using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Friendship;
using Backend.Core.Features.Ranking.Models;
using Backend.Database;
using Backend.Database.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Ranking.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimsPrincipal _principal;
        private readonly FriendsService _friendsService;

        public RankingsController(IUnitOfWork unitOfWork, ClaimsPrincipal principal, FriendsService friendsService)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _friendsService = friendsService;
        }

        [HttpGet("global")]
        public async Task<IEnumerable<UserResponse>> GetGlobalAsync()
        {
            var users = await _unitOfWork.GetAllAsync<User>();
            var results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("local")]
        public async Task<IEnumerable<UserResponse>> GetLocalAsync(string zip)
        {
            IEnumerable<User> users = await _unitOfWork.WhereAsync<User>(u => u.Location.Zip == zip);

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
            var user = await _unitOfWork.GetAsync<User>(_principal.Id());
            var zipCode = user.Location?.Zip ?? "3000";

            var allUsers = await _unitOfWork.GetAllAsync<User>();
            var global = CreateResult(allUsers).OrderByDescending(u => u.Points);
            var local = CreateResult(allUsers.Where(u => u.Location != null && u.Location.Zip == zipCode)).OrderByDescending(u => u.Points);
            var friends = CreateResult(await _friendsService.GetFriends()).OrderByDescending(u => u.Points);

            return new RankingSummary
            {
                FriendRank = GetRank(friends, user),
                GlobalRank = GetRank(global, user),
                LocalRank = GetRank(local, user)
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