using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Extensions;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.UserManagement
{
    public class UserService
    {
        private readonly ClaimsPrincipal _principal;

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, ClaimsPrincipal principal)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
        }

        public Task<User> GetCurrentUser() => _unitOfWork.GetByIdOrDefaultAsync<User>(_principal.Id());

        public async Task<User> GetByEmailAsync(string email) => await _unitOfWork.GetByEmailAsync(email);

        public async Task<bool> IsRegisteredAsync(string email) => await _unitOfWork.GetByEmailAsync(email) != null;
    }
}