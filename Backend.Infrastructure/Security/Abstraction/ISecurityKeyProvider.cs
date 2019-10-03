using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Security.Abstraction
{
    public interface ISecurityKeyProvider
    {
        SymmetricSecurityKey GetSecurityKey();
    }
}