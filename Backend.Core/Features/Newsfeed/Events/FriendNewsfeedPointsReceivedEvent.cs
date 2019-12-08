using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class FriendNewsfeedPointsReceivedEvent : FriendNewsfeedEvent
    {
        public FriendNewsfeedPointsReceivedEvent(string displayName, IEnumerable<Guid> friends, int points)
            : base(friends)
        {
            Title = "Neue Punkte";
            Message = "Dein Freund " + displayName + " hat " + points + " Suffizienz Punkte erhalten!";
        }
    }
}