using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Backend.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.UserManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        private readonly UserService _userService;

        public UsersController(
            IPasswordStorage passwordStorage,
            ISecurityTokenFactory securityTokenFactory,
            UserService userService)
        {
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
                string token = await _userService.RegisterAsync(email);
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