using Backend.Core.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Web.StartupTask
{
    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this IWebHost webHost, CancellationToken cancellationToken)
        {
            // Load all tasks from DI
            using (var scope = webHost.Services.CreateScope())
            {
                var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

                // Execute all the tasks
                foreach (var startupTask in startupTasks)
                {
                    await startupTask.ExecuteAsync(cancellationToken);
                }

                // Start the tasks as normal
                await webHost.RunAsync(cancellationToken);
            }
        }
    }
}
