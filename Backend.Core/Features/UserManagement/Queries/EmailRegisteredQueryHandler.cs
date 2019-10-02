using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class EmailRegisteredQueryHandler : ISubscriber
    {
        private readonly IReader _reader;

        public EmailRegisteredQueryHandler(IReader reader) => _reader = reader;

        public async Task<EmailRegisteredQueryResult> ExecuteAsync(EmailRegisteredQuery query)
        {
            long count = await _reader.CountAsync<User>(u => u.Email == query.Email.ToLowerInvariant());
            return new EmailRegisteredQueryResult { IsRegistered = count > 0 };
        }
    }
}