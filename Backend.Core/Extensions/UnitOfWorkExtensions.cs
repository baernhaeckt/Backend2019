using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<User> GetByEmailAsync(this IUnitOfWork unitOfWork, string email) => await unitOfWork.SingleOrDefaultAsync<User>(u => u.Email == email);

        public static async Task<IEnumerable<Token>> GetTokensFromUser(this IUnitOfWork unitOfWork, Guid id) => await unitOfWork.WhereAsync<Token>(t => t.UserId == id);
    }
}