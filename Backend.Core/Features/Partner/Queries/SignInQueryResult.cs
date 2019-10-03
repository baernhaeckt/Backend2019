namespace Backend.Core.Features.Partner.Queries
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
    }
}