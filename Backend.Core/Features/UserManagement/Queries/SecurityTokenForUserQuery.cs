using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Queries
{
    public class SecurityTokenForUserQuery : IQuery<SecurityTokenForUserQueryResult>
    {
        public SecurityTokenForUserQuery(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}