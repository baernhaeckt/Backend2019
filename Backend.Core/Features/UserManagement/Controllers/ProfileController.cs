using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.UserManagement.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserService _userService;

        private readonly ICommandPublisher _commandPublisher;

        public ProfileController(UserService userService, ICommandPublisher commandPublisher)
        {
            _userService = userService;
            _commandPublisher = commandPublisher;
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
        public async Task Update([FromBody] UserUpdateRequest userUpdateRequest) => await _userService.Update(userUpdateRequest);

        [HttpPatch("password")]
        public async Task ChangePassword([FromBody] ChangePasswordModel model)
        {
            var command = new ChangePasswordCommand(HttpContext.User.Id(), model.NewPassword, model.OldPassword);
            await _commandPublisher.ExecuteAsync(command);
        }
    }
}