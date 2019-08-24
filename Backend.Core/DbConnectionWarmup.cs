using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Database;
using MongoDB.Driver;

namespace Backend.Core
{
    public class DbConnectionWarmup
    {
        private readonly IMongoOperation<User> _mongoOperation;

        public DbConnectionWarmup(IMongoOperation<User> mongoOperation)
        {
            _mongoOperation = mongoOperation;
        }

        public void Run()
        {
            Task.Run(() => _ = _mongoOperation.GetQuerableAsync().FirstOrDefault());
        }
    }
}
