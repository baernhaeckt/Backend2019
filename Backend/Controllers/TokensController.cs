using AspNetCore.MongoDB;
using Backend.Entities;
using Backend.Extensions;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMongoOperation<Token> _operation;

        public TokensController(IMongoOperation<Token> operation)
        {
            _operation = operation;
        }

        [HttpGet]
        public ActionResult<TokenResponse> Get(Guid tokenGuid)
        {
            Token token = _operation.GetQuerableAsync().SingleOrDefault(t => t.Value == tokenGuid);
            if(token == null)
            {
                return NotFound();
            }

            
            return new TokenResponse()
            {
                Id = token.Id,
                Text = token.Text,
                Points = token.Points,
                Valid = token.Valid,
            };
        }

        [HttpPost]
        public ActionResult Post(Guid tokenGuid)
        {
            var token = _operation.GetQuerableAsync().SingleOrDefault(t => t.Value == tokenGuid);
            if (token == null)
            {
                return NotFound();
            }

            if (token.Valid)
            {
                token.UserId = User.Id();
            }

            _operation.UpdateAsync(token.Id, token);

            return Ok();
        }
    }
}