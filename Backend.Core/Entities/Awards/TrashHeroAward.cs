namespace Backend.Core.Entities.Awards
{
    public class TrashHeroAward : Award
    {
        public TrashHeroAward()
        {
            Name = "Trash Hero";
            Title = "Ohne Verpackung unterwegs";
            Description = "Du hast unnötige Verpackungen eingespart. Vielen Dank!";
            Kind = AwardKind.TrashHero;
        }
    }
}