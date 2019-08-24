namespace Backend.Database
{
    public class TrashHeroAward : Award
    {
        public TrashHeroAward()
        {
            Title = "Ohne Verpackung unterwegs";
            Description = "Du hast unnötige Verpackungen eingespart. Vielen Dank!";
            Kind = AwardKind.TrashHero;
        }
    }
}