using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Entities.Awards
{
    public class Award : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Title { get; protected set; } = string.Empty;

        public string Description { get; protected set; } = string.Empty;

        public string Kind => GetType().Name;
    }
}