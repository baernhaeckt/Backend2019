using System.Threading.Tasks;

namespace Backend.Infrastructure.Email.Abstraction
{
    public interface IEmailService
    {
        Task Send(string subject, string text, string receiver);
    }
}