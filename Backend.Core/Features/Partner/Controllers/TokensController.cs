using System;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Core.Extensions;
using Backend.Core.Features.Partner.Commands;
using Backend.Core.Features.UserManagement.Security;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Partner.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    [Authorize(Roles = Roles.Partner)]
    public class TokensController : ControllerBase
    {
        private readonly TokenService _tokenGenerationService;

        private readonly ICommandPublisher _commandPublisher;

        public TokensController(TokenService tokenGenerationService, ICommandPublisher commandPublisher)
        {
            _tokenGenerationService = tokenGenerationService;
            _commandPublisher = commandPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get(Guid partnerId)
        {
            return await _tokenGenerationService.GenerateForPartnerAsync(partnerId);
        }

        [HttpPost]
        public async Task PostAsync(Guid tokenGuid)
        {
            var command = new RewardForUserTokenCommand(tokenGuid, HttpContext.User.Id());
            await _commandPublisher.ExecuteAsync(command);
        }
    }
}