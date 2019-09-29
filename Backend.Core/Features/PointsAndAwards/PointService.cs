using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Core.Features.Newsfeed;
using Backend.Core.Features.Newsfeed.Abstraction;
using Backend.Core.Features.Newsfeed.Events;
using Backend.Core.Features.PointsAndAwards.Models;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.PointsAndAwards
{
    public class PointService
    {
        private readonly AwardService _awardService;

        private readonly IEventFeed _eventFeed;

        private readonly ClaimsPrincipal _principal;

        private readonly IUnitOfWork _unitOfWork;

        public PointService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, IEventFeed eventFeed, AwardService awardService)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _eventFeed = eventFeed;
            _awardService = awardService;
        }

        public async Task AddPoints(Token token)
        {
            User user = await _unitOfWork.GetAsync<User>(_principal.Id());
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
            User user = await _unitOfWork.GetAsync<User>(_principal.Id());
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

        public async Task<IEnumerable<PointAction>> PointHistory(Guid id)
        {
            User user = await _unitOfWork.GetAsync<User>(id);
            return user.PointActions;
        }

        private async Task Process(int points, double co2saving, User user)
        {
            user.Points += points;
            user.Co2Saving += co2saving;

            await _unitOfWork.UpdateAsync(user);

            // TODO: Encouple this, using Silverback!
            // Fire and forget.
            await _eventFeed.PublishAsync(new PointsReceivedNewsfeedEvent(user, points));
            await _eventFeed.PublishAsync(new FriendNewsfeedPointsReceivedEvent(user, points));

            await _awardService.CheckForNewAwardsAsync(user);
        }
    }
}