using AspNetCore.MongoDB;

namespace Backend.Database
{
    public class Location : IMongoEntity
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}