namespace Backend.Core.Features.UserManagement.Queries
{
    public class SecurityTokenForUserQueryResult
    {
        public SecurityTokenForUserQueryResult(string token) => Token = token;

        public string Token { get; }
    }
}