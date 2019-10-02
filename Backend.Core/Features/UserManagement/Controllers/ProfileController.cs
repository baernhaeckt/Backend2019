using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.UserManagement.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        private readonly IQueryPublisher _queryPublisher;

        public ProfileController(ICommandPublisher commandPublisher, IQueryPublisher queryPublisher)
        {
            _commandPublisher = commandPublisher;
            _queryPublisher = queryPublisher;
        }

        [HttpGet]
        public async Task<UserProfileQueryResult> Get()
        {
            return await _queryPublisher.ExecuteAsync(new UserProfileQuery(HttpContext.User.Id()));
        }

        [HttpPatch]
        public async Task Update([FromBody] ProfileUpdateModel model)
        {
            var command = new UpdateProfileCommand(HttpContext.User.Id(), model.DisplayName);
            await _commandPublisher.ExecuteAsync(command);
        }

        [HttpPatch("password")]
        public async Task ChangePassword([FromBody] ChangePasswordModel model)
        {
            var command = new ChangePasswordCommand(HttpContext.User.Id(), model.NewPassword, model.OldPassword);
            await _commandPublisher.ExecuteAsync(command);
        }
    }
}