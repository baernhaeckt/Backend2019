using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.UserManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        private readonly IQueryPublisher _queryPublisher;

        public UsersController(IQueryPublisher queryPublisher, ICommandPublisher commandPublisher)
        {
            _queryPublisher = queryPublisher;
            _commandPublisher = commandPublisher;
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Register([FromBody] RegisterUserRequest request)
        {
            EmailRegisteredQueryResult result = await _queryPublisher.ExecuteAsync(new EmailRegisteredQuery(request.Email));
            if (!result.IsRegistered)
            {
                await _commandPublisher.ExecuteAsync(new RegisterUserCommand(request.Email));
                SecurityTokenForUserQueryResult tokenResult = await _queryPublisher.ExecuteAsync(new SecurityTokenForUserQuery(request.Email));
                return new UserLoginResponse { Token = tokenResult.Token };
            }

            return new UserLoginResponse();
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> Login([FromBody] UserLoginRequest request)
        {
            SignInQueryResult result = await _queryPublisher.ExecuteAsync(new SignInQuery(request.Email, request.Password));
            if (result.UserNotFound)
            {
                return NotFound();
            }

            if (result.PasswordNotCorrect)
            {
                return Forbid();
            }

            return new ActionResult<UserLoginResponse>(new UserLoginResponse { Token = result.Token });
        }
    }
}