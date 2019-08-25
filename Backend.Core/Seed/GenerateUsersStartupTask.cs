using AspNetCore.MongoDB;
using Backend.Core.Security.Abstraction;
using Backend.Core.Startup;
using Backend.Database;
using Bogus;
using Bogus.Locations;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Core.Seed
{
    public class GenerateUsersStartupTask : IStartupTask
    {
        private const int SeedCount = 80;

        private readonly IPasswordStorage _passwordStorage;
        private readonly IMongoOperation<User> _userRepository;
        private readonly IPaswordGenerator _paswordGenerator;

        public GenerateUsersStartupTask(IPasswordStorage passwordStorage, IMongoOperation<User> userRepository, IPaswordGenerator paswordGenerator)
        {
            _passwordStorage = passwordStorage;
            _userRepository = userRepository;
            _paswordGenerator = paswordGenerator;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_userRepository.Count(FilterDefinition<User>.Empty) != 0)
            {
                return;
            }

            IList<string> zips = new[] { "3001", "3006", "3010", "3013", "3018", "3027", "3004", "3007", "3011", "3014" };

            Faker<Location> locationFaker = new Faker<Location>()
                .RuleFor(u => u.Zip, f => f.PickRandom(zips))
                .RuleFor(u => u.City, "Bern")
                .RuleFor(u => u.Latitude, f => f.Location().AreaCircle(46.944699, 7.443788, 10000).Latitude)
                .RuleFor(u => u.Longitude, f => f.Location().AreaCircle(46.944699, 7.443788, 10000).Longitude);

            var locations = locationFaker.Generate(100).ToList();

            Faker<User> faker = new Faker<User>()
                .RuleFor(u => u.Id, f => ObjectId.GenerateNewId().ToString())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, _passwordStorage.Create(_paswordGenerator.Generate()))
                .RuleFor(u => u.DisplayName, f => f.Name.FirstName())
                .RuleFor(u => u.Location, f => f.PickRandom(locations));

            List<User> users = faker.Generate(SeedCount);

            // Random generate friendships
            var random = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < 12; i++)
                {
                    int index = random.Next(users.Count);
                    user.Friends.Add(users[index].Id);
                }
            }

            await _userRepository.InsertManyAsync(users);
        }
    }
}