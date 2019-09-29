using System.Threading;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Hosting.Abstraction
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}