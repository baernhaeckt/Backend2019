using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Infrastructure.Email.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.EventSubscriber
{
    public class UserRegisteredEventSubscriber : ISubscriber
    {
        private readonly IEmailService _emailService;

        public UserRegisteredEventSubscriber(IEmailService emailService) => _emailService = emailService;

        public async Task ExecuteAsync(UserRegisteredEvent @event)
        {
            await _emailService.Send("Password", "Your password: " + @event.PlainTextPassword, @event.User.Email);
        }
    }
}