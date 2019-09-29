using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Abstraction;
using Backend.Database.Entities;
using Backend.Database.Framework;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Backend.Core.Features.UserManagement.Data
{
    public class MongoDbSetupStartupTask : IStartupTask
    {
        private readonly DbContextFactory _dbContextFactory;

        public MongoDbSetupStartupTask(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            DbContext dbContext = _dbContextFactory.Create();

            await EnsureEmailUniqueIndex(dbContext, cancellationToken);
        }

        private static async Task EnsureEmailUniqueIndex(DbContext dbContext, CancellationToken cancellationToken)
        {
            var emailIndexExists = false;
            IAsyncCursor<BsonDocument> indexes = dbContext.GetCollection<User>().Indexes.List();
            List<BsonDocument> list = await indexes.ToListAsync(cancellationToken);
            foreach (BsonDocument bsonDocument in list)
            {
                if (bsonDocument.Elements.Any(e => e.Name == "key" && e.Value.AsBsonDocument.Names.FirstOrDefault() == nameof(User.Email)))
                {
                    emailIndexExists = true;
                }
            }

            if (!emailIndexExists)
            {
                await dbContext.GetCollection<User>().Indexes.CreateOneAsync(new CreateIndexModel<User>("{ \"" + nameof(User.Email) + "\": 1 }", new CreateIndexOptions { Unique = true }), null, cancellationToken);
            }
        }
    }
}