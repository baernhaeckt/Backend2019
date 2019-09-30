using System;

namespace Backend.Infrastructure.Persistence.Abstraction
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type type)
        {
            Name = type.Name;
        }

        public EntityNotFoundException()
        {
            Name = string.Empty;
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
            Name = string.Empty;
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            Name = string.Empty;
        }

        public string Name { get; }
    }
}