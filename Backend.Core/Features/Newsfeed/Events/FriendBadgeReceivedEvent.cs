using Backend.Database;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class FriendBadgeReceivedEvent : FriendEvent
    {
        public FriendBadgeReceivedEvent(User user, Award award) :
            base(user)
        {
            title = "Neuer Award";
            message = "Dein Freund " + user.DisplayName + " hat den Award '" + award.Name + "' erhalten!";
        }
    }
}