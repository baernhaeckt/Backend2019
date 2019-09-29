using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Infrastructure.Hosting.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Web
{
    public class StartupTaskRunner : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public StartupTaskRunner(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Load all tasks from DI
            using IServiceScope scope = _serviceProvider.CreateScope();
            IEnumerable<IStartupTask> startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

            // Execute all the tasks
            foreach (IStartupTask startupTask in startupTasks)
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}