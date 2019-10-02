using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class RegisterUserCommand : ICommand
    {
        public RegisterUserCommand(string email) => Email = email;

        public string Email { get; }
    }
}