using System.Threading;
using System.Threading.Tasks;

namespace Backend.Web.StartupTask
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
