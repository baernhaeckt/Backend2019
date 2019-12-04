using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Queries
{
    internal class EmailRegisteredQueryHandler : QueryHandler<EmailRegisteredQueryResult, EmailRegisteredQuery>
    {
        public EmailRegisteredQueryHandler(IReader reader, ILogger<EmailRegisteredQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<EmailRegisteredQueryResult> ExecuteAsync(EmailRegisteredQuery query)
        {
            Logger.ExecuteEmailRegisteredQueryHandler(query.Email);

            long count = await Reader.CountAsync<User>(u => u.Email == query.Email.ToLowerInvariant());

            if (count == 0)
            {
                Logger.NoUserWithThisEmailFound(query.Email);
            }
            else
            {
                Logger.FoundAlreadyRegisteredUsers(query.Email, count);
            }

            return new EmailRegisteredQueryResult { IsRegistered = count > 0 };
        }
    }
}