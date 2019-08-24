using AspNetCore.MongoDB;
using Backend.Database;
using Backend.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Web.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMongoOperation<User> _operation;

        public ProfileController(IMongoOperation<User> operation)
        {
            _operation = operation;
        }

        [HttpGet]
        public PrivateUserResponse Current()
        {
            // TODO: Implement.
            throw new NotImplementedException();
        }
    }
}