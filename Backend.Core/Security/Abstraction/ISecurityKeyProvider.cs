using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Security.Abstraction
{
    public interface ISecurityKeyProvider
    {
        SymmetricSecurityKey GetSecurityKey();
    }
}