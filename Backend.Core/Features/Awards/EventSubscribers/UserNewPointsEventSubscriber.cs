using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Awards;
using Backend.Core.Events;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Awards.EventSubscribers
{
    internal class UserNewPointsEventSubscriber : EventSubscriber<UserNewPointsEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IUnitOfWork _unitOfWork;

        public UserNewPointsEventSubscriber(IUnitOfWork unitOfWork, ILogger<UserNewPointsEventSubscriber> logger, IEventPublisher eventPublisher)
            : base(logger)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public override async Task ExecuteAsync(UserNewPointsEvent @event)
        {
            Logger.HandleUserNewPointsEvent(@event.UserId);

            User user = await _unitOfWork.GetByIdOrThrowAsync<User, User>(@event.UserId, user => new User
            {
                DisplayName = user.DisplayName,
                PointHistory = user.PointHistory,
                Awards = user.Awards
            });

            IList<Award> newAwards = new List<Award>();
            if (user.PointHistory.Count >= 1
                && user.Awards.All(a => !(a is OnBoardingAward)))
            {
                // This is the first token the user gets, so this is worth an award.
                var award = new OnBoardingAward();
                newAwards.Add(award);

                Logger.GrantAward(@event.UserId, award.Kind);
            }

            if (user.PointHistory.Count(t => t.Type == SufficientType.Packing) >= 2
                && user.Awards.All(a => !(a is TrashHeroAward)))
            {
                var award = new TrashHeroAward();
                newAwards.Add(award);

                Logger.GrantAward(@event.UserId, award.Kind);
            }

            if (newAwards.Any())
            {
                await _unitOfWork.UpdateAsync<User>(@event.UserId, new { Awards = newAwards });

                foreach (Award newAward in newAwards)
                {
                    await _eventPublisher.PublishAsync(new UserNewAwardEvent(@event.UserId, user.DisplayName, newAward.Title));
                }
            }

            Logger.HandleUserNewPointsEventSuccessful(@event.UserId, newAwards.Count);
        }
    }
}