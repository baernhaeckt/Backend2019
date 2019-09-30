using System;
using System.Threading.Tasks;
using Backend.Core.Features.PointsAndAwards.Commands;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.PointsAndAwards.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        public TokensController(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        [HttpPost]
        public async Task PostAsync(Guid tokenGuid)
        {
            await _commandPublisher.ExecuteAsync(new TokenRewardCommand(tokenGuid));
        }
    }
}