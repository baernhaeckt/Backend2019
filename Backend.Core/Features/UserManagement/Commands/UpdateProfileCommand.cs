using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Commands
{
    public class UpdateProfileCommand : ICommand
    {
        public UpdateProfileCommand(Guid id, string displayName, string city, string street, string postalCode)
        {
            Id = id;
            DisplayName = displayName;
            City = city;
            Street = street;
            PostalCode = postalCode;
        }

        public string DisplayName { get; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public Guid Id { get; }
    }
}