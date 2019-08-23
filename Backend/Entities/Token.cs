using AspNetCore.MongoDB;
using System;

namespace Backend.Entities
{
    public class Token : IMongoEntity
    {
        public string Text { get; set; }

        public int Points { get; set; }

        public string UserId { get; set; }

        public bool Valid => UserId != null;

        public Guid Value { get; set; }
    }
}
