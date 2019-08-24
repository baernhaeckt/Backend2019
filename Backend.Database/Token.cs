﻿using AspNetCore.MongoDB;
using System;

namespace Backend.Database
{
    public class Token : IMongoEntity
    {
        public string Text { get; set; }

        public int Points { get; set; }

        public double Co2Saving { get; set; }

        public string UserId { get; set; }

        public bool Valid => UserId == null;

        public Guid Value { get; set; }

        public SufficientType SufficientType { get; set; }
    }
}
