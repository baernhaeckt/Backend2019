using Backend.Core.Startup;
using Backend.Database;
using Backend.Database.Abstraction;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Security;
using Backend.Core.Features.UserManagement.Security.Abstraction;

namespace Backend.Core.Features.UserManagement.Data
{
    public class AdminUsersStartupTask : IStartupTask
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AdminUsersStartupTask(IUnitOfWork unitOfWork, IPasswordStorage passwordStorage, IPasswordGenerator paswordGenerator, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (await _unitOfWork.SingleOrDefaultAsync<User>(u => u.Email == _configuration["AdminEmail"]) != null)
            {
                return;
            }

            var user = new User();
            user.Email = _configuration["AdminEmail"];
            user.Password = _configuration["AdminPassword"];
            user.DisplayName = "Admin";
            user.Roles = new List<string> { Roles.Administrator };
            await _unitOfWork.InsertAsync(user);
        }
    }
}