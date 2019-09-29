using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Abstraction;
using Backend.Core.Features.UserManagement.Security;
using Backend.Database.Abstraction;
using Backend.Database.Entities;
using Microsoft.Extensions.Configuration;

namespace Backend.Core.Features.UserManagement.Data
{
    public class AdminUsersStartupTask : IStartupTask
    {
        private readonly IConfiguration _configuration;

        private readonly IUnitOfWork _unitOfWork;

        public AdminUsersStartupTask(IUnitOfWork unitOfWork, IConfiguration configuration)
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