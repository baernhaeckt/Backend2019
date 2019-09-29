using Backend.Core.Entities;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class FriendNewsfeedPointsReceivedEvent : FriendNewsfeedEvent
    {
        public FriendNewsfeedPointsReceivedEvent(User user, int points)
            : base(user)
        {
            Title = "Neue Punkte";
            Message = "Dein Freund " + user.DisplayName + " hat " + points + " Suffizienz Punkte erhalten!";
        }
    }
}