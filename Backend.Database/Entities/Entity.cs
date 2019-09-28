using MongoDbGenericRepository.Models;
using System;

namespace Backend.Database
{
    public abstract class Entity : IDocument
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
