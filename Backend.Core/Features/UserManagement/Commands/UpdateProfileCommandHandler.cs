using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class UpdateProfileCommandHandler : ISubscriber
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task ExecuteAsync(UpdateProfileCommand command)
        {
            await _unitOfWork.UpdateAsync<User>(command.Id, new { command.DisplayName });
        }
    }
}