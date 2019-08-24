using AspNetCore.MongoDB;
using Backend.Core.Newsfeed;
using Backend.Core.Security.Abstraction;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public UsersController(IPaswordGenerator passwordGenerator, IPasswordStorage passwordStorage, ISecurityTokenFactory securityTokenFactory, IMongoOperation<User> operation, IEventStream eventStream)
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
            IEnumerable<User> users = _operation.GetQuerableAsync().Where(u => u.Email == email);
            if (users.Count() == 0)
            {
                string newPassword = _passwordGenerator.Generate();
                var newUser = new User { Email = email, Password = _passwordStorage.Create(newPassword) };
                await _operation.InsertOneAsync(newUser);

                string token = _securityTokenFactory.Create(newUser);
                return new LoginResponse { Token = token };
            }
            if (users.Count() == 1)
            {
                // This user is already registered.
                // Show password input form.
                return new LoginResponse();
            }
         
            // This means invalid data.
            return BadRequest();
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public ActionResult<LoginResponse> Login(string email, string password)
        {
            IEnumerable<User> users = _operation.GetQuerableAsync().Where(u => u.Email == email);
            if (users.Count() == 0)
            {
                return NotFound();
            }

            if (users.Count() == 1)
            {
                if (!_passwordStorage.Match(password, users.Single().Password))
                {
                    return Forbid();
                }

                string securityToken = _securityTokenFactory.Create(users.Single());
                return new ActionResult<LoginResponse>(new LoginResponse { Token = securityToken });
            }

            // This means invalid data.
            return BadRequest();
        }
    }
}