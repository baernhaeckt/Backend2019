using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Email;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.EventSubscriber
{
    public class UserRegisteredEventSubscriber : EventHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;

        public UserRegisteredEventSubscriber(IEmailService emailService, ILogger<UserRegisteredEventSubscriber> logger)
            : base(logger) => _emailService = emailService;

        public override async Task ExecuteAsync(UserRegisteredEvent @event)
        {
            Logger.UserHandleUserRegisteredEvent(@event.User.Id);

            await _emailService.Send("Password", "Your password: " + @event.PlainTextPassword, @event.User.Email);

            Logger.UserHandleUserRegisteredEventSuccessful(@event.User.Id);
        }
    }
}