namespace Backend.Core.Entities.Awards
{
    public class Award
    {
        public string Name { get; set; } = string.Empty;

        public string Title { get; protected set; } = string.Empty;

        public string Description { get; protected set; } = string.Empty;

        public string Kind => GetType().Name;
    }
}