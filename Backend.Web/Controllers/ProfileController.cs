using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public PrivateUserResponse Get()
        {
            User user = _userService.CurrentUser;
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
            _userService.Update(userUpdateRequest);
        }
    }
}