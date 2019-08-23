namespace Backend.Core.Security
{
    public interface IPasswordStorage
    {
        string Create(string password);

        bool Match(string inputPassword, string originalPassword);
    }
}
