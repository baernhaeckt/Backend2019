using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MongoDB;

namespace Backend.Models
{
    public class LoginResponse : IMongoEntity
    {
        public string Token { get; set; }
    }
}
