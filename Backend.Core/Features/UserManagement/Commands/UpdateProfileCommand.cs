using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class UpdateProfileCommand : ICommand
    {
        public UpdateProfileCommand(Guid id, object displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public object DisplayName { get; }

        public Guid Id { get; }
    }
}