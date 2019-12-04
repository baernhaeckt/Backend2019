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
    }
}