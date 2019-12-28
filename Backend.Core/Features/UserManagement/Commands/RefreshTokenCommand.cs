using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class RefreshTokenCommand : ICommand<RefreshTokenCommandResult>
    {
        public RefreshTokenCommand(string token) => Token = token;

        public string Token { get; }
    }
}