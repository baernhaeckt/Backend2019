using System.Threading;
using System.Threading.Tasks;

namespace Backend.Core.Abstraction
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}