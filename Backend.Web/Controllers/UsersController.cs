using Backend.Core.Security.Abstraction;
using Backend.Core.Services;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IPasswordStorage _passwordStorage;
        private readonly ISecurityTokenFactory _securityTokenFactory;
        private readonly UserService _userService;

        public FriendsService FriendService { get; }

        public UsersController(
            IPasswordStorage passwordStorage,
            ISecurityTokenFactory securityTokenFactory,
            FriendsService friendService,
            UserService userService)
        {
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
            FriendService = friendService;
            _userService = userService;
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Register(string email)
        {
            if (_userService.IsRegistered(email))
            {
                string token = await _userService.RegisterAsync(email);
                return new LoginResponse { Token = token };
            }
            else
            {
                return new LoginResponse();
            }
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public ActionResult<LoginResponse> Login(string email, string password)
        {
            if (_userService.IsRegistered(email))
            {
                User user = _userService.GetByEmail(email);
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