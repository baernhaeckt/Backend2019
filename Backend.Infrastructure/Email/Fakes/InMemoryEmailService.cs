using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Infrastructure.Email.Abstraction;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Email.Fakes
{
    public class InMemoryEmailService : IEmailService
    {
        private readonly ILogger<InMemoryEmailService> _logger;

        private readonly IList<EmailMessage> _messages = new List<EmailMessage>();

        public InMemoryEmailService(ILogger<InMemoryEmailService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<EmailMessage> Messages => _messages;

        public Task Send(string subject, string text, string receiver)
        {
            _logger.SendEmail(subject, text, receiver);
            _messages.Add(new EmailMessage(subject, text, receiver));
            return Task.CompletedTask;
        }
    }
}