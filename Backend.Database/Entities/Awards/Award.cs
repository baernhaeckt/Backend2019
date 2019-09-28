

namespace Backend.Database
{
    public class Award : Entity
    {
        public string Name { get; set; }

        public string Title { get; protected set; }

        public AwardKind Kind { get; protected set; }

        public string Description { get; protected set; }
    }
}