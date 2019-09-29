using System;
using Backend.Core.Entities;

namespace Backend.Core.Features.Newsfeed.Events
{
    public abstract class FriendNewsfeedEvent : NewsfeedEvent
    {
        protected FriendNewsfeedEvent(User user)
        {
            Variant = "info";
            foreach (Guid friend in user.Friends)
            {
                Audience.Add(friend);
            }
        }
    }
}