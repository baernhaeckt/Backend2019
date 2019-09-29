using System;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Core.Features.Newsfeed.Hubs
{
    [Authorize]
    public class NewsfeedHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Guid userId = Context.User.Id();

            Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

            return base.OnConnectedAsync();
        }
    }
}