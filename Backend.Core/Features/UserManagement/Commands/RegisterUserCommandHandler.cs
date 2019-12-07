using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Events;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class RegisterUserCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly IPasswordGenerator _passwordGenerator;

        private readonly IPasswordStorage _passwordStorage;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterUserCommandHandler> logger, IPasswordGenerator passwordGenerator, IPasswordStorage passwordStorage, IEventPublisher eventPublisher)
            : base(unitOfWork, logger)
        {
            _passwordGenerator = passwordGenerator;
            _passwordStorage = passwordStorage;
            _eventPublisher = eventPublisher;
        }

        public override async Task ExecuteAsync(RegisterUserCommand command)
        {
            Logger.ExecuteUserRegistration(command.Email);

            string newPassword = _passwordGenerator.Generate();
            var newUser = new User
            {
                Email = command.Email.ToLowerInvariant(), // TODO: Validate E-Mail Address
                PasswordHash = _passwordStorage.Create(newPassword),
                DisplayName = "Newby",
                Roles = new List<string> { Roles.User },
                Location = new Location
                {
                    City = "Bern",
                    PostalCode = "3011",
                    Latitude = 46.944699,
                    Longitude = 7.443788
                }
            };
            newUser = await UnitOfWork.InsertAsync(newUser);

            Logger.ExecuteUserRegistrationSuccessful(newUser.Id, command.Email);

            await _eventPublisher.PublishAsync(new UserRegisteredEvent(newUser, newPassword));
        }
    }
}