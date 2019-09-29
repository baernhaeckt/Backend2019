namespace Backend.Database.Entities.Awards
{
    public class Award : Entity
    {
        public string Name { get; set; } = string.Empty;

        public string Title { get; protected set; } = string.Empty;

        public AwardKind Kind { get; protected set; }

        public string Description { get; protected set; } = string.Empty;
    }
}