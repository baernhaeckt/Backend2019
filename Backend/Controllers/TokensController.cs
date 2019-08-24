using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        [AllowAnonymous]
        public async Task<ActionResult<string>> Get(Guid partnerId)
        {
            if (partnerId != Guid.Parse("ccc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                return BadRequest();
            }

            Token token = new Token();
            token.Points = 10;
            token.CreatedDate = DateTime.Now;
            token.Text = "Blabla";
            token.Value = Guid.NewGuid();

            await _operation.InsertOneAsync(token);

            return token.Value.ToString();
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