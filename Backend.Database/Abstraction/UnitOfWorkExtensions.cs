using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Database.Entities;

namespace Backend.Database.Abstraction
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<User> GetByEmailAsync(this IUnitOfWork unitOfWork, string email)
        {
            return await unitOfWork.SingleOrDefaultAsync<User>(u => u.Email == email);
        }

        public static async Task<IEnumerable<User>> GetByZipAsync(this IUnitOfWork unitOfWork, string zip)
        {
            return await unitOfWork.WhereAsync<User>(u => u.Location.Zip == zip);
        }

        public static async Task<IEnumerable<Token>> GetTokensFromUser(this IUnitOfWork unitOfWork, Guid id)
        {
            return await unitOfWork.WhereAsync<Token>(t => t.UserId == id);
        }
    }
}