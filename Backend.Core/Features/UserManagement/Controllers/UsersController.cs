using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Security.Abstraction;
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

        private readonly IPasswordStorage _passwordStorage;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        private readonly UserService _userService;

        public UsersController(ICommandPublisher commandPublisher, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory, UserService userService)
        {
            _commandPublisher = commandPublisher;
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
            _userService = userService;
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Register(string email)
        {
            if (!await _userService.IsRegisteredAsync(email))
            {
                await _commandPublisher.ExecuteAsync(new RegisterUserCommand(email));
                User user = await _userService.GetByEmailAsync(email);
                string token = _securityTokenFactory.Create(user);
                return new LoginResponse { Token = token };
            }

            return new LoginResponse();
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(string email, string password)
        {
            if (await _userService.IsRegisteredAsync(email))
            {
                User user = await _userService.GetByEmailAsync(email);
                if (!_passwordStorage.Match(password, user.Password))
                {
                    return Forbid();
                }

                string securityToken = _securityTokenFactory.Create(user);
                return new ActionResult<LoginResponse>(new LoginResponse { Token = securityToken });
            }

            return NotFound();
        }
    }
}