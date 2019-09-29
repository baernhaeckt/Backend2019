using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.UserManagement.Models;
using Backend.Core.Features.UserManagement.Security;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Backend.Database.Abstraction;
using Backend.Database.Entities;

namespace Backend.Core.Features.UserManagement
{
    public class UserService
    {
        private readonly IPasswordGenerator _passwordGenerator;

        private readonly IPasswordStorage _passwordStorage;

        private readonly ClaimsPrincipal _principal;

        private readonly ISecurityTokenFactory _securityTokenFactory;

        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, ClaimsPrincipal principal, ISecurityTokenFactory securityTokenFactory, IPasswordGenerator passwordGenerator, IPasswordStorage passwordStorage)
        {
            _unitOfWork = unitOfWork;
            _principal = principal;
            _securityTokenFactory = securityTokenFactory;
            _passwordGenerator = passwordGenerator;
            _passwordStorage = passwordStorage;
        }

        public Task<User> GetCurrentUser()
        {
            return _unitOfWork.GetAsync<User>(_principal.Id());
        }

        public async Task Update(UserUpdateRequest updateUserRequest)
        {
            User user = await _unitOfWork.GetAsync<User>(_principal.Id());
            user.DisplayName = updateUserRequest.DisplayName;
            await _unitOfWork.UpdateAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _unitOfWork.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetByPlzAsync(string zip)
        {
            return await _unitOfWork.GetByZipAsync(zip);
        }

        public async Task<bool> IsRegisteredAsync(string email)
        {
            return await _unitOfWork.GetByEmailAsync(email) != null;
        }

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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _unitOfWork.GetAllAsync<User>();
        }
    }
}