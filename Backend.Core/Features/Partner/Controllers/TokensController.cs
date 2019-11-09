using System;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Partner.Commands;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Partner.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        public TokensController(ICommandPublisher commandPublisher) => _commandPublisher = commandPublisher;

        [HttpGet]
        [Authorize(Roles = Roles.Partner)]
        public async Task<ActionResult<string>> Get(string tokenType)
        {
            var command = new CreateNewTokenCommand(HttpContext.User.Id(), tokenType);
            return (await _commandPublisher.ExecuteAsync(command)).ToString();
        }

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task PostAsync(Guid tokenGuid)
        {
            var command = new RewardUserTokenCommand(tokenGuid, HttpContext.User.Id());
            await _commandPublisher.ExecuteAsync(command);
        }
    }
}