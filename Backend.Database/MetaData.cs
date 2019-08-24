using AspNetCore.MongoDB;

namespace Backend.Database
{
    public class MetaData : IMongoEntity
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}