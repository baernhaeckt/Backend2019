using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Features.UserManagement.Security.Abstraction
{
    public interface ISecurityKeyProvider
    {
        SymmetricSecurityKey GetSecurityKey();
    }
}