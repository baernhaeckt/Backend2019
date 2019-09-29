using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Security;
using Backend.Infrastructure.Hosting.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;
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

            var user = new User
            {
                Email = _configuration["AdminEmail"],
                Password = _configuration["AdminPassword"],
                DisplayName = "Admin",
                Roles = new List<string> { Roles.Administrator }
            };
            await _unitOfWork.InsertAsync(user);
        }
    }
}