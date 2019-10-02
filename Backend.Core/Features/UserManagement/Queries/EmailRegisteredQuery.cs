using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Queries
{
    public class EmailRegisteredQuery : IQuery<EmailRegisteredQueryResult>
    {
        public EmailRegisteredQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}