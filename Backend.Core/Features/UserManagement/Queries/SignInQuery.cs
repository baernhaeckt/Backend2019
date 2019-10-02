using Silverback.Messaging.Messages;

namespace Backend.Core.Features.UserManagement.Queries
{
    public class SignInQuery : IQuery<SignInQueryResult>
    {
        public string Email { get; }

        public string Password { get; }

        public SignInQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}