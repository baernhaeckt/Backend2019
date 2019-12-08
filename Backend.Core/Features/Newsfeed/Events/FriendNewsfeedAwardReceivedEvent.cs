using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class FriendNewsfeedAwardReceivedEvent : FriendNewsfeedEvent
    {
        public FriendNewsfeedAwardReceivedEvent(string displayName, IEnumerable<Guid> friends, string awardName)
            : base(friends)
        {
            Title = "Neuer Award";
            Message = "Dein Freund " + displayName + " hat den Award '" + awardName + "' erhalten!";
        }
    }
}