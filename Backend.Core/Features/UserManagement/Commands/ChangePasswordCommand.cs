using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class ChangePasswordCommand : ICommand
    {
        public ChangePasswordCommand(Guid userId, string newPassword, string oldPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
            OldPassword = oldPassword;
        }

        public Guid UserId { get; }

        public string NewPassword { get; }

        public string OldPassword { get; }
    }
}