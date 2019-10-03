using System;
using System.Threading.Tasks;
using Backend.Core.Features.Partner.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Partner.Controllers
{
    [Route("api/tokenIssuers")]
    [ApiController]
    public class TokenIssuersController : ControllerBase
    {
        private readonly IQueryPublisher _queryPublisher;

        public TokenIssuersController(IQueryPublisher queryPublisher) => _queryPublisher = queryPublisher;

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<string> Login(Guid id, string secret)
        {
            SignInQueryResult result = await _queryPublisher.ExecuteAsync(new SignInQuery(id, secret));
            return result.Token;
        }
    }
}