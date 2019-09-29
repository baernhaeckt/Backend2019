namespace Backend.Core.Features.PointsAndAwards.Models
{
    public class PointAwarding
    {
        public PointAwardingKind Source { get; set; }

        public int Points { get; set; }

        public string Text { get; set; } = string.Empty;

        public double Co2Saving { get; set; }
    }
}