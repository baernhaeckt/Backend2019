

namespace Backend.Database
{
    public class SufficientType : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int BaselinePoint { get; set; }

        public double BaselineCo2Saving { get; set; }
    }
}
