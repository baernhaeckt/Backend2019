using Backend.Database;

namespace Backend.Core.Security.Abstraction
{
    public interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}