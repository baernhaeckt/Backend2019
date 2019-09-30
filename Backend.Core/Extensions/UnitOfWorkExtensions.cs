using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<User> GetByEmailAsync(this IUnitOfWork unitOfWork, string email) => await unitOfWork.SingleOrDefaultAsync<User>(u => u.Email == email);
    }
}