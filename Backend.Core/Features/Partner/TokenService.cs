using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.PointsAndAwards;
using Backend.Database;
using Backend.Database.Abstraction;

namespace Backend.Core.Features.Partner
{
    public class TokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly PointService _pointService;

        public TokenService(IUnitOfWork unitOfWork, ClaimsPrincipal claimsPrincipal, PointService pointService)
        {
            _unitOfWork = unitOfWork;
            _claimsPrincipal = claimsPrincipal;
            _pointService = pointService;
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
                await _unitOfWork.InsertAsync(token);
                return token.Value.ToString();
            }

            throw new WebException("Partner doesn't exist.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task AssignTokenToUserAsync(Guid tokenGuid)
        {
            Token token = await _unitOfWork.SingleOrDefaultAsync<Token>(t => t.Value == tokenGuid);
            if (token == null)
            {
                throw new WebException("Token not found.", System.Net.HttpStatusCode.NotFound);
            }

            if (!token.Valid)
            {
                throw new WebException("Token already used.", System.Net.HttpStatusCode.BadRequest);
            }

            token.UserId = _claimsPrincipal.Id();
            await _unitOfWork.UpdateAsync(token);
            await _pointService.AddPoints(token);
        }
    }
}
