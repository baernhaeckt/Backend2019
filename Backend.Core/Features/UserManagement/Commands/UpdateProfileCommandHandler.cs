using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework.Cqrs;
using Backend.Infrastructure.Abstraction.Geolocation;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class UpdateProfileCommandHandler : CommandHandler<UpdateProfileCommand>
    {
        private readonly IGeocodingService _geocodingService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProfileCommandHandler> logger, IGeocodingService geocodingService)
            : base(unitOfWork, logger) => _geocodingService = geocodingService;

        public override async Task ExecuteAsync(UpdateProfileCommand command)
        {
            Logger.ExecuteUserProfileUpdate(command);

            LookupResult result = await _geocodingService.LookupAsync(command.PostalCode, command.City, command.Street);

            if (result.Failed)
            {
                // Save all other infos than location, also if the location couldn't be resolved
                await UnitOfWork.UpdateAsync<User>(command.Id, new { command.DisplayName });

                Logger.UserUnableToLookupAddress(command.City, command.Street, command.PostalCode);

                throw new ValidationException("Address lookup failed.");
            }

            await UnitOfWork.UpdateAsync<User>(command.Id, new
            {
                command.DisplayName,
                Location = new Location
                {
                    City = command.City,
                    PostalCode = command.PostalCode,
                    Street = command.Street,
                    Longitude = result.Longitude,
                    Latitude = result.Latitude
                }
            });

            Logger.ExecuteUserProfileUpdateSuccessful(command.Id);
        }
    }
}