using Microsoft.IdentityModel.Tokens;

namespace Backend.Core.Security
{
    public interface ISecurityKeyProvider
    {
        SymmetricSecurityKey GetSecurityKey();
    }
}