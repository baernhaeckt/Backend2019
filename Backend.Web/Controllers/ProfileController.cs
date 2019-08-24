using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMongoOperation<User> _operation;

        public ProfileController(IMongoOperation<User> operation)
        {
            _operation = operation;
        }

        [HttpGet]
        public async Task<PrivateUserResponse> CurrentAsync()
        {
            User user = await _operation.GetByIdAsync(User.Id());
            return new PrivateUserResponse
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Location = new LocationResponse
                {
                    Latitude = user.Location.Latitude,
                    Longitude = user.Location.Longitude
                },
                Points = user.Points,
            };
        }
    }
}