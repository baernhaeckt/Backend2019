using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Hosting.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.UserManagement.Data
{
    public class SetIndexOnEmailStartupTask : IStartupTask
    {
        private readonly IIndexCreator _indexCreator;

        public SetIndexOnEmailStartupTask(IIndexCreator indexCreator) => _indexCreator = indexCreator;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await _indexCreator.Create<User>(nameof(User.Email), cancellationToken);
        }
    }
}