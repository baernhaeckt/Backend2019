using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class ChangePasswordCommandHandler : CommandHandler<ChangePasswordCommand>
    {
        private readonly IPasswordStorage _passwordStorage;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangePasswordCommandHandler> logger, IPasswordStorage passwordStorage)
            : base(unitOfWork, logger) => _passwordStorage = passwordStorage;

        public override async Task ExecuteAsync(ChangePasswordCommand command)
        {
            Logger.ExecuteUserPasswordChange(command.UserId);

            string passwordHash = await UnitOfWork.GetByIdOrThrowAsync<User, string>(command.UserId, u => u.PasswordHash);
            if (!_passwordStorage.Match(command.OldPassword, passwordHash))
            {
                Logger.UserPasswordChangeOldPasswordNotMatched(command.UserId);
                throw new ValidationException("Incorrect password.");
            }

            object definition = new { PasswordHash = _passwordStorage.Create(command.NewPassword) };
            await UnitOfWork.UpdateAsync<User>(command.UserId, definition);

            Logger.ExecuteUserPasswordChangeChangeSuccessful(command.UserId);
        }
    }
}