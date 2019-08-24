using AspNetCore.MongoDB;

namespace Backend.Database
{
    public class SufficientType : IMongoEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int BaselinePoint { get; set; }
    }
}
