using Backend.Database;

namespace Backend.Core.Features.UserManagement.Security.Abstraction
{
    public interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}