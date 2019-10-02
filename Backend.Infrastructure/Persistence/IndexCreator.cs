using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Infrastructure.Persistence.Abstraction;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Backend.Infrastructure.Persistence
{
    internal class IndexCreator : IIndexCreator
    {
        private readonly DbContextFactory _dbContextFactory;

        public IndexCreator(DbContextFactory dbContextFactory) => _dbContextFactory = dbContextFactory;

        public async Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : Entity, new()
        {
            DbContext dbContext = _dbContextFactory.Create();

            var indexExists = false;
            IAsyncCursor<BsonDocument> indexes = dbContext.GetCollection<TEntity>().Indexes.List();
            List<BsonDocument> list = await indexes.ToListAsync(cancellationToken);
            foreach (BsonDocument bsonDocument in list)
            {
                if (bsonDocument.Elements.Any(e => e.Name == "key" && e.Value.AsBsonDocument.Names.FirstOrDefault() == field))
                {
                    indexExists = true;
                }
            }

            if (!indexExists)
            {
                await dbContext.GetCollection<TEntity>().Indexes.CreateOneAsync(new CreateIndexModel<TEntity>("{ \"" + field + "\": 1 }", new CreateIndexOptions { Unique = true }), null, cancellationToken);
            }
        }
    }
}