using System;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Partner.Commands;
using Backend.Core.Features.UserManagement.Security;
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

        private readonly TokenService _tokenGenerationService;

        public TokensController(TokenService tokenGenerationService, ICommandPublisher commandPublisher)
        {
            _tokenGenerationService = tokenGenerationService;
            _commandPublisher = commandPublisher;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Partner)]
        public async Task<ActionResult<string>> Get(Guid partnerId) => await _tokenGenerationService.GenerateForPartnerAsync(partnerId);

        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task PostAsync(Guid tokenGuid)
        {
            var command = new RewardForUserTokenCommand(tokenGuid, HttpContext.User.Id());
            await _commandPublisher.ExecuteAsync(command);
        }
    }
}