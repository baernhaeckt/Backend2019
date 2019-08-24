﻿using AspNetCore.MongoDB;
using Backend.Database;
using System.Linq;
using System.Security.Claims;

namespace Backend.Web.Services
{
    public abstract class PersonalizedService
    {
        protected string currentUserId => Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        protected ClaimsPrincipal Principal { get; }

        protected IMongoOperation<User> userRepository { get; }

        protected User CurrentUser
            => userRepository.GetQuerableAsync().Single(u => u.Email == currentUserId);

        protected PersonalizedService(IMongoOperation<User> userRepository, ClaimsPrincipal principal)
        {
            this.userRepository = userRepository;
            Principal = principal;
        }
    }
}
