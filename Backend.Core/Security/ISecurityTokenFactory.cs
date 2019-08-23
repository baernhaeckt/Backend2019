using Backend.Models.Database;

namespace Backend.Core.Security
{
    public interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}