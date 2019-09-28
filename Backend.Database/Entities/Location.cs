﻿
using MongoDbGenericRepository.Models;

namespace Backend.Database
{
    public class Location : Entity
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }
    }
}