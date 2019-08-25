using Backend.Core.Security.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Backend.Core.Newsfeed
{
    [Authorize]
    public class NewsfeedHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string userId = Context.User.Id();

            Groups.AddToGroupAsync(Context.ConnectionId, userId);

            return base.OnConnectedAsync();
        }
    }
}