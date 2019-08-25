using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class TokenService
    {
        private readonly IMongoOperation<Token> _tokenRepository;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly UserService _userService;

        public TokenService(IMongoOperation<Token> tokenRepository, ClaimsPrincipal claimsPrincipal, UserService userService)
        {
            _tokenRepository = tokenRepository;
            _claimsPrincipal = claimsPrincipal;
            _userService = userService;
        }

        public async Task<string> GenerateForPartnerAsync(Guid partnerId)
        {
            Token token = null;
            if (partnerId == Guid.Parse("ccc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Partner = "Palette";
                token.Points = 10;
                token.Co2Saving = 1;
                token.CreatedDate = DateTime.Now;
                token.Text = "Einkauf in der Palette";
                token.Value = Guid.NewGuid();
                token.SufficientType = new SufficientType
                {
                    Title = "Verpackung",
                    Description = "Du hast Verpackungslos eingekauft."
                };
            }
            else if (partnerId == Guid.Parse("bcc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Partner = "Mein Nachbar";
                token.Points = 5;
                token.Co2Saving = 0.1;
                token.CreatedDate = DateTime.Now;
                token.Text = "Nachbar über Suffizienz aufgeklärt";
                token.Value = Guid.NewGuid();
                token.SufficientType = new SufficientType
                {
                    Title = "Wissen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt."
                };
            }
            else if (partnerId == Guid.Parse("acc14b11-5922-4e3e-bb54-03e71facaeb3"))
            {
                token = new Token();
                token.Partner = "Meine Schwester";
                token.Points = 15;
                token.Co2Saving = 2;
                token.CreatedDate = DateTime.Now;
                token.Text = "Du Teilst deine Auto mit deiner Schwester";
                token.Value = Guid.NewGuid();
                token.SufficientType = new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast deinen Besitz mit anderen geteilt."
                };
            }

            if (token != null)
            {
                await _tokenRepository.InsertOneAsync(token);
                return token.Value.ToString();
            }

            throw new WebException("Partner doesn't exist.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task AssignTokenToUserAsync(Guid tokenGuid)
        {
            var token = _tokenRepository.GetQuerableAsync().SingleOrDefault(t => t.Value == tokenGuid);
            if (token == null)
            {
                throw new WebException("Token not found.", System.Net.HttpStatusCode.NotFound);
            }

            if (!token.Valid)
            {
                throw new WebException("Token already used.", System.Net.HttpStatusCode.BadRequest);
            }


            token.UserId = _claimsPrincipal.Id();
            await _tokenRepository.UpdateAsync(token.Id, token);
            await _userService.AddPoints(token);
        }
    }
}
