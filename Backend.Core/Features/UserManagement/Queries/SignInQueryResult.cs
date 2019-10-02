namespace Backend.Core.Features.UserManagement.Queries
{
    public class SignInQueryResult
    {
        public bool UserNotFound { get; }

        public bool PasswordNotCorrect { get; }

        public string Token { get; }

        public SignInQueryResult(bool userNotFound, bool passwordNotCorrect, string? token)
        {
            UserNotFound = userNotFound;
            PasswordNotCorrect = passwordNotCorrect;
            Token = token;
        }
    }
}