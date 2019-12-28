namespace Backend.Core.Features.UserManagement.Queries
{
    public class SignInQueryResult
    {
        public SignInQueryResult(bool userNotFound, bool passwordNotCorrect, string token)
        {
            UserNotFound = userNotFound;
            PasswordNotCorrect = passwordNotCorrect;
            Token = token;
        }

        public bool UserNotFound { get; }

        public bool PasswordNotCorrect { get; }

        public string Token { get; }

        public string RefreshToken { get; }
    }
}