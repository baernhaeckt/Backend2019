using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class ChangePasswordCommandHandler : ISubscriber
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordStorage passwordStorage)
        {
            _unitOfWork = unitOfWork;
            _passwordStorage = passwordStorage;
        }

        public async Task ExecuteAsync(ChangePasswordCommand command)
        {
            string passwordHash = await _unitOfWork.GetByIdOrThrowAsync<User, string>(command.UserId, u => u.PasswordHash);
            if (!_passwordStorage.Match(command.OldPassword, passwordHash))
            {
                throw new ValidationException("Incorrect password.");
            }

            object definition = new { PasswordHash = _passwordStorage.Create(command.NewPassword) };
            await _unitOfWork.UpdateAsync<User>(command.UserId, definition);
        }
    }
}