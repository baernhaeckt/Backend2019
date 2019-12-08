using System;
using Backend.Core.Features.Newsfeed.Events;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Newsfeed
{
    public static class Logging
    {
        public static void PublishEventToNewsFeed(this ILogger logger, NewsfeedEvent @event)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Publish new event to signalR newsfeed. Title: {title}, Audience: {audience}", @event.Title, string.Join(',', @event.Audience));
        }

        public static void HandleNewAwardEvent(this ILogger logger, Guid userId, string awardName)
        {
            logger.LogTrace(new EventId(2, typeof(Logging).Namespace), "Handle NewAwardEvent: Publish to newsfeed. UserId: {userId}, AwardName: {awardName}", userId, awardName);
        }

        public static void HandleNewAwardEventSuccessful(this ILogger logger, Guid userId, string awardName)
        {
            logger.LogTrace(new EventId(2, typeof(Logging).Namespace), "Handled NewAwardEvent. {userId}, AwardName: {awardName}", userId, awardName);
        }

        public static void HandleUserNewPointsEvent(this ILogger logger, Guid userId, int points, double co2Savings)
        {
            logger.LogTrace(new EventId(4, typeof(Logging).Namespace), "Handle NewUserNewPointsEvent. Title: {userId}, Points: {points}, Co2Savings: {co2Savings}", userId, points, co2Savings);
        }

        public static void HandleUserNewPointsEventSuccessful(this ILogger logger, Guid userId, int points, double co2Savings)
        {
            logger.LogTrace(new EventId(5, typeof(Logging).Namespace), "Handled NewUserNewPointsEvent. Title: {userId}, Points: {points}, Co2Savings: {co2Savings}", userId, points, co2Savings);
        }
    }
}