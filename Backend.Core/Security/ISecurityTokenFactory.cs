using Backend.Models.Database;

namespace Backend.Core.Security
{
    interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}
