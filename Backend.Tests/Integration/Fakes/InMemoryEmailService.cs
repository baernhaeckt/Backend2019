using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Infrastructure.Email.Abstraction;

namespace Backend.Tests.Integration.Fakes
{
    public class InMemoryEmailService : IEmailService
    {
        private readonly IList<EmailMessage> _messages = new List<EmailMessage>();

        public IEnumerable<EmailMessage> Messages => _messages;

        public Task Send(string subject, string text, string receiver)
        {
            _messages.Add(new EmailMessage(subject, text, receiver));
            return Task.CompletedTask;
        }
    }
}
