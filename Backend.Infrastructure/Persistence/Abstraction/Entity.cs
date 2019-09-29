using System;
using MongoDbGenericRepository.Models;

namespace Backend.Infrastructure.Persistence.Abstraction
{
    public abstract class Entity : IDocument
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid Id { get; set; }

        public int Version { get; set; }
    }
}