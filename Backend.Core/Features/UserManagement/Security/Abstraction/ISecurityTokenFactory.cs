using Backend.Core.Entities;

namespace Backend.Core.Features.UserManagement.Security.Abstraction
{
    public interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}