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
        private readonly IQueryPublisher _queryPublisher;

        private readonly ICommandPublisher _commandPublisher;

        public UsersController(IQueryPublisher queryPublisher, ICommandPublisher commandPublisher)
        {
            _queryPublisher = queryPublisher;
            _commandPublisher = commandPublisher;
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Register(string email)
        {
            EmailRegisteredQueryResult result = await _queryPublisher.ExecuteAsync(new EmailRegisteredQuery(email));
            if (!result.IsRegistered)
            {
                await _commandPublisher.ExecuteAsync(new RegisterUserCommand(email));
                var tokenResult = await _queryPublisher.ExecuteAsync(new SecurityTokenForUserQuery(email));
                return new LoginResponse { Token = tokenResult.Token };
            }

            return new LoginResponse();
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(string email, string password)
        {
            SignInQueryResult result = await _queryPublisher.ExecuteAsync(new SignInQuery(email, password));
            if (result.UserNotFound)
            {
                return NotFound();
            }

            if (result.PasswordNotCorrect)
            {
                return Forbid();
            }

            return new ActionResult<LoginResponse>(new LoginResponse { Token = result.Token });
        }
    }
}