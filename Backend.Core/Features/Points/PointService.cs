using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Core.Extensions;
using Backend.Core.Features.Points.Models;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Points
{
    public class PointService
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly ClaimsPrincipal _principal;

        private readonly IUnitOfWork _unitOfWork;

        public PointService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _eventPublisher = eventPublisher;
        }

        public async Task AddPoints(Token token)
        {
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(_principal.Id());
            user.PointActions.Add(new PointAction
            {
                Point = token.Points,
                Action = token.Text,
                Co2Saving = token.Co2Saving,
                SponsorRef = token.Partner,
                SufficientType = new UserSufficientType
                {
                    Title = token.SufficientType.Title
                }
            });

            await Process(token.Points, token.Co2Saving, user);
        }

        public async Task AddPoints(PointAwarding pointAwarding)
        {
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(_principal.Id());
            user.PointActions.Add(new PointAction
            {
                Point = pointAwarding.Points,
                Action = pointAwarding.Text,
                Co2Saving = pointAwarding.Co2Saving,
                SponsorRef = pointAwarding.Source.ToString(),
                SufficientType = new UserSufficientType
                {
                    Title = "Wissen"
                }
            });

            await Process(pointAwarding.Points, pointAwarding.Co2Saving, user);
        }

        public async Task<IEnumerable<PointAction>> GetPointHistory(Guid id)
        {
            User user = await _unitOfWork.GetByIdOrDefaultAsync<User>(id);
            return user.PointActions;
        }

        private async Task Process(int points, double co2Saving, User user)
        {
            user.Points += points;
            user.Co2Saving += co2Saving;

            await _unitOfWork.UpdateAsync(user);

            await _eventPublisher.PublishAsync(new UserNewPointsEvent(user, points, co2Saving));
        }
    }
}