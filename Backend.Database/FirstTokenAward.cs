namespace Backend.Database
{
    public class FirstTokenAward : Award
    {
        public FirstTokenAward()
        {
            Title = "First Token";
            Kind = AwardKind.FirstLogin;
        }
    }
}