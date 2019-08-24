using Backend.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Web.StartupTask
{
    public class WarmupStartupTask : IStartupTask
    {
        private readonly DbConnectionWarmup _dbConnectionWarmup;

        public WarmupStartupTask(DbConnectionWarmup dbConnectionWarmup)
        {
            _dbConnectionWarmup = dbConnectionWarmup;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _dbConnectionWarmup.Run();
            return Task.CompletedTask;
        }
    }
}
