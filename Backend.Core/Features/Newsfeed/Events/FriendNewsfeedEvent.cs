using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class FriendNewsfeedEvent : NewsfeedEvent
    {
        protected FriendNewsfeedEvent(IEnumerable<Guid> friends)
        {
            Variant = "info";
            foreach (Guid friend in friends)
            {
                Audience.Add(friend);
            }
        }
    }
}