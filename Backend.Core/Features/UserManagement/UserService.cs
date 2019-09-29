using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Models;
using Backend.Database;
using Backend.Database.Abstraction;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Security;
using Backend.Core.Features.UserManagement.Security.Abstraction;

namespace Backend.Core.Features.UserManagement
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ClaimsPrincipal _principal;
        private readonly ISecurityTokenFactory _securityTokenFactory;

        public Task<User> GetCurrentUser() => _unitOfWork.GetAsync<User>(_principal.Id());

        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPasswordStorage _passwordStorage;

        public UserService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, ISecurityTokenFactory securityTokenFactory, IPasswordGenerator passwordGenerator, IPasswordStorage passwordStorage)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _securityTokenFactory = securityTokenFactory;
            _passwordGenerator = passwordGenerator;
            _passwordStorage = passwordStorage;
        }

        public async Task Update(UserUpdateRequest updateUserRequest)
        {
            var user = await _unitOfWork.GetAsync<User>(_principal.Id());
            user.DisplayName = updateUserRequest.DisplayName;
            await _unitOfWork.UpdateAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email) => await _unitOfWork.GetByEmailAsync(email);

        public async Task<IEnumerable<User>> GetByPlzAsync(string zip) => await _unitOfWork.GetByZipAsync(zip);

        public async Task<bool> IsRegisteredAsync(string email) => await _unitOfWork.GetByEmailAsync(email) != null;

        public async Task<string> RegisterAsync(string email)
        {
            string newPassword = _passwordGenerator.Generate();
            var newUser = new User
            {
                Email = email,
                Password = _passwordStorage.Create(newPassword),
                DisplayName = "Newby",
                Roles = new List<string> { Roles.User },
                Location = new Location
                {
                    City = "Bern",
                    Zip = "3011",
                    Latitude = 46.944699,
                    Longitude = 7.443788
                }
            };
            newUser = await _unitOfWork.InsertAsync(newUser);

            string token = _securityTokenFactory.Create(newUser);
            return token;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _unitOfWork.GetAllAsync<User>();
    }
}
