namespace Backend.Core.Features.UserManagement.Security.Abstraction
{
    public interface IPasswordGenerator
    {
        string Generate();
    }
}