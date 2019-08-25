using System.Threading;
using System.Threading.Tasks;

namespace Backend.Core.Startup
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
