using Backend.Core.Services;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/users/{userId}/points")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        public PointsController(UserService userService)
        {
            UserService = userService;
        }

        public UserService UserService { get; }

        [HttpGet]
        public async Task<IEnumerable<PointResponse>> GetAsync(string userId)
        {
            var pointList = await UserService.PointHistory(userId);
            return pointList.OrderByDescending(p => p.Date).Take(25).Select(p => new PointResponse
            {
                Date = p.Date,
                Id = p.Id,
                Text = p.Action,
                Value = p.Point,
                Co2Saving = p.Co2Saving,
                SufficientType = p.SufficientType
            }).ToList();
        }
    }
}