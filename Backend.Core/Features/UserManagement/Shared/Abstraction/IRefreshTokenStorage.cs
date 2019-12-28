using System;
using System.Threading.Tasks;
using Backend.Core.Entities;

namespace Backend.Core.Features.UserManagement.Shared.Abstraction
{
    internal interface IRefreshTokenStorage
    {
        Task<string> Create(Guid userId);

        Task<RefreshToken?> Retrieve(string tokenValue);
    }
}