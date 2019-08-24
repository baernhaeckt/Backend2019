using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Core.Services;
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
        public ProfileController(UserService userService)
        {
            UserService = userService;
        }

        public UserService UserService { get; }

        [HttpGet]
        public PrivateUserResponse Current()
        {
            User user = UserService.CurrentUser;
            return new PrivateUserResponse
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Location = new LocationResponse
                {
                    Latitude = user.Location?.Latitude ?? 0.0,
                    Longitude = user.Location?.Longitude ?? 0.0
                },
                Points = user.Points,
            };
        }

        [HttpPatch]
        public void Update(UserUpdateRequest userUpdateRequest)
        {

        }
    }
}