using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Geolocation.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class UpdateProfileCommandHandler : ISubscriber
    {
        private readonly IGeocodingService _geocodingService;

        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IGeocodingService geocodingService, ILogger<UpdateProfileCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _geocodingService = geocodingService;
            _logger = logger;
        }

        public async Task ExecuteAsync(UpdateProfileCommand command)
        {
            LookupResult result = await _geocodingService.LookupAsync(command.PostalCode, command.City, command.Street);

            if (result.Failed)
            {
                // Save all other infos than location, also if the location couldn't be resolved
                await _unitOfWork.UpdateAsync<User>(command.Id, new { command.DisplayName });

                _logger.UnableToLookupAddress(command.City, command.Street, command.PostalCode);

                throw new ValidationException("Address lookup failed.");
            }

            await _unitOfWork.UpdateAsync<User>(command.Id, new
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
        }
    }
}