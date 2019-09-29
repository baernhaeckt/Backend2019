namespace Backend.Core.Features.Baseline.Models
{
    public class BaselineResponse
    {
        public string Title { get; set; } = string.Empty;

        public int BaseLinePoint { get; set; }

        public double BaselineCo2Saving { get; set; }
    }
}