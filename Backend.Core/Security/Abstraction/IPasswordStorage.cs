namespace Backend.Core.Security.Abstraction
{
    public interface IPasswordStorage
    {
        string Create(string password);

        bool Match(string password, string goodHash);
    }
}
