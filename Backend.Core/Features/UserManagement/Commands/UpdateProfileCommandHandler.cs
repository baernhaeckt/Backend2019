using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Geolocation;
using Backend.Infrastructure.Persistence.Abstraction;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.UserManagement.Commands
{
    internal class UpdateProfileCommandHandler : ISubscriber
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGeolocationService _geolocationService;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork, IGeolocationService _geolocationService)
        {
            _unitOfWork = unitOfWork;
            this._geolocationService = _geolocationService;
        }

        public async Task ExecuteAsync(UpdateProfileCommand command)
        {
            ReverseGeolocationResult result = await _geolocationService.Reverse(command.PostalCode, command.City, command.Street);

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