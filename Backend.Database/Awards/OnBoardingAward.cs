namespace Backend.Database
{
    public class OnBoardingAward : Award
    {
        public OnBoardingAward()
        {
            Name = "Member";
            Title = "Deine ersten Punkte";
            Description = "Du hast deine ersten Punkte gesammelt. Willkommen an Board!";
            Kind = AwardKind.Onboarding;
        }
    }
}