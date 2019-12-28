namespace Backend.Core.Features.UserManagement.Queries
{
    public class EmailRegisteredQueryResult
    {
        public EmailRegisteredQueryResult(bool isRegistered) => IsRegistered = isRegistered;

        public bool IsRegistered { get; }
    }
}