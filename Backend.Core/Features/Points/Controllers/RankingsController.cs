using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Core.Features.Friendship;
using Backend.Core.Features.Points.Models;
using Backend.Infrastructure.Persistence.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Points.Controllers
{
    [Route("api/rankings")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly FriendsService _friendsService;

        private readonly ClaimsPrincipal _principal;

        private readonly IUnitOfWork _unitOfWork;

        public RankingsController(IUnitOfWork unitOfWork, ClaimsPrincipal principal, FriendsService friendsService)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _friendsService = friendsService;
        }

        [HttpGet("global")]
        public async Task<IEnumerable<UserResponse>> GetGlobalAsync()
        {
            IEnumerable<User> users = await _unitOfWork.GetAllAsync<User>();
            IEnumerable<UserResponse> results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("local")]
        public async Task<IEnumerable<UserResponse>> GetLocalAsync(string zip)
        {
            IEnumerable<User> users = await _unitOfWork.WhereAsync<User>(u => u.Location.PostalCode == zip);

            IEnumerable<UserResponse> results = CreateResult(users);

            return results.OrderByDescending(r => r.Points);
        }

        [HttpGet("friends")]
        public async Task<IEnumerable<UserResponse>> GetFriendsAsync()
        {
            IEnumerable<UserResponse> results = CreateResult(await _friendsService.GetFriends());

            return results.OrderByDescending(u => u.Points);
        }

        [HttpGet("summary")]
        public async Task<RankingSummary> GetSummary()
        {
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(_principal.Id());
            string zipCode = user.Location?.PostalCode ?? "3000";

            IEnumerable<User> allUsers = await _unitOfWork.GetAllAsync<User>();
            IOrderedEnumerable<UserResponse> global = CreateResult(allUsers).OrderByDescending(u => u.Points);
            IOrderedEnumerable<UserResponse> local = CreateResult(allUsers.Where(u => u.Location != null && u.Location.PostalCode == zipCode)).OrderByDescending(u => u.Points);
            IOrderedEnumerable<UserResponse> friends = CreateResult(await _friendsService.GetFriends()).OrderByDescending(u => u.Points);

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
            foreach (UserResponse u in list)
            {
                if (u.Id == user.Id)
                {
                    break;
                }

                rank++;
            }

            return rank;
        }

        private static IEnumerable<UserResponse> CreateResult(IEnumerable<User> users)
        {
            users = users.ToList();
            IList<UserResponse> results = new List<UserResponse>(users.Count());

            foreach (User user in users)
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