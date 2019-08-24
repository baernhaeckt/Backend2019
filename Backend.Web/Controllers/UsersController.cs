using AspNetCore.MongoDB;
using Backend.Core.Security.Abstraction;
using Backend.Database;
using Backend.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Web.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IPaswordGenerator _passwordGenerator;
        private readonly IPasswordStorage _passwordStorage;
        private readonly ISecurityTokenFactory _securityTokenFactory;
        private readonly IMongoOperation<User> _operation;

        public UsersController(IPaswordGenerator passwordGenerator, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory, IMongoOperation<User> operation)
        {
            _passwordGenerator = passwordGenerator;
            _passwordStorage = passwordStorage;
            _securityTokenFactory = securityTokenFactory;
            _operation = operation;
        }

        [HttpPost(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Register(string email)
        {
            User user = _operation.GetQuerableAsync().SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                return BadRequest();
            }

            string newPassword = _passwordGenerator.Generate();
            var newUser = new User { Email = email, Password = _passwordStorage.Create(newPassword) };
            await _operation.InsertOneAsync(newUser);

            string token = _securityTokenFactory.Create(newUser);
            return new LoginResponse { Token = token };
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public ActionResult<LoginResponse> Login(string email, string password)
        {
            User user = _operation.GetQuerableAsync().SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            if (!_passwordStorage.Match(password, user.Password))
            {
                return Forbid();
            }

            string securityToken = _securityTokenFactory.Create(user);
            return new ActionResult<LoginResponse>(new LoginResponse { Token = securityToken });
        }
    }
}