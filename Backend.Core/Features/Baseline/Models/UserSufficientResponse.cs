namespace Backend.Core.Features.Baseline.Models
{
    public class UserSufficientResponse
    {
        public string Title { get; set; } = string.Empty;

        public int Point { get; set; }

        public double Co2Saving { get; set; }
    }
}