using System.Threading.Tasks;

namespace Backend.Infrastructure.Abstraction.Email
{
    public interface IEmailService
    {
        Task Send(string subject, string text, string receiver);
    }
}