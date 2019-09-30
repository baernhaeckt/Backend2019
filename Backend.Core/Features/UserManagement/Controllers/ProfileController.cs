using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.UserManagement.Controllers
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
        public async Task<PrivateUserResponse> Get()
        {
            User user = await _userService.GetCurrentUser();
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
                Points = user.Points
            };
        }

        [HttpPatch]
        public async Task Update([FromBody] UserUpdateRequest userUpdateRequest)
        {
            await _userService.Update(userUpdateRequest);
        }
    }
}