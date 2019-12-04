using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Backend.Infrastructure.Persistence
{
    internal class IndexCreator : IIndexCreator
    {
        private readonly DbContextFactory _dbContextFactory;

        private readonly ILogger<IndexCreator> _logger;

        public IndexCreator(DbContextFactory dbContextFactory, ILogger<IndexCreator> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task Create<TEntity>(string field, CancellationToken cancellationToken)
            where TEntity : IEntity, new()
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
                IMongoCollection<TEntity> collection = dbContext.GetCollection<TEntity>();

                _logger.IndexCreatorCreateIndexFor(field, typeof(TEntity).FullName, collection.CollectionNamespace.FullName, collection.Database.DatabaseNamespace.DatabaseName);
                await collection.Indexes.CreateOneAsync(new CreateIndexModel<TEntity>("{ \"" + field + "\": 1 }", new CreateIndexOptions { Unique = true }), null, cancellationToken);
            }
        }
    }
}