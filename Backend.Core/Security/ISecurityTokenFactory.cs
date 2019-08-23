using Backend.Models.Database;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Security
{
    public interface ISecurityTokenFactory
    {
        string Create(User user);
    }
}