using System;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class OwnNewsfeedEvent : NewsfeedEvent
    {
        protected OwnNewsfeedEvent(Guid userId)
        {
            Audience.Add(userId);
            Variant = "success";
        }
    }
}