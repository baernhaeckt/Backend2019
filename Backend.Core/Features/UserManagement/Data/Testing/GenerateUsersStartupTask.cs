using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;
using Bogus;
using Bogus.Locations;

namespace Backend.Core.Features.UserManagement.Data.Testing
{
    public class GenerateUsersStartupTask : IStartupTask
    {
        private const int SeedCount = 20;

        private readonly IPasswordGenerator _passwordGenerator;

        private readonly IPasswordStorage _passwordStorage;

        private readonly IUnitOfWork _unitOfWork;

        public GenerateUsersStartupTask(IUnitOfWork unitOfWork, IPasswordStorage passwordStorage, IPasswordGenerator passwordGenerator)
        {
            _unitOfWork = unitOfWork;
            _passwordStorage = passwordStorage;
            _passwordGenerator = passwordGenerator;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var users = new List<User>();

            // This users are for the automated tests.
            if (await _unitOfWork.CountAsync<User>(u => u.Email == TestCredentials.User1) < 1)
            {
                users.Add(new User
                {
                    Email = TestCredentials.User1,
                    Roles = new List<string> { Roles.User },
                    PasswordHash = _passwordStorage.Create(TestCredentials.User1Password)
                });
            }

            if (await _unitOfWork.CountAsync<User>(u => u.Email == TestCredentials.User2) < 1)
            {
                users.Add(new User
                {
                    Email = TestCredentials.User2,
                    Roles = new List<string> { Roles.User },
                    PasswordHash = _passwordStorage.Create(TestCredentials.User2Password),
                    Location = new Location
                    {
                        City = "Bern",
                        PostalCode = "3011",
                        Latitude = 46.944699,
                        Longitude = 7.443788
                    }
                });
            }

            if (await _unitOfWork.CountAsync<User>(u => u.Email == TestCredentials.User3) < 1)
            {
                users.Add(new User
                {
                    Email = TestCredentials.User3,
                    Roles = new List<string> { Roles.User },
                    PasswordHash = _passwordStorage.Create(TestCredentials.User3Password),
                    Location = new Location
                    {
                        City = "Bern",
                        PostalCode = "3011",
                        Latitude = 46.944699,
                        Longitude = 7.443788
                    }
                });
            }

            if (await _unitOfWork.CountAsync<User>() <= SeedCount)
            {
                IList<string> zips = new[] { "3001", "3006", "3010", "3013", "3018", "3027", "3004", "3007", "3014" };

                Faker<Location> locationFaker = new Faker<Location>()
                    .RuleFor(u => u.PostalCode, f => f.PickRandom(zips))
                    .RuleFor(u => u.City, "Bern")
                    .RuleFor(u => u.Latitude, f => f.Location().AreaCircle(46.944699, 7.443788, 10000).Latitude)
                    .RuleFor(u => u.Longitude, f => f.Location().AreaCircle(46.944699, 7.443788, 10000).Longitude);

                List<Location> locations = locationFaker.Generate(100).ToList();

                Faker<User> faker = new Faker<User>()
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.PasswordHash, _passwordStorage.Create(_passwordGenerator.Generate()))
                    .RuleFor(u => u.DisplayName, f => f.Name.FirstName())
                    .RuleFor(u => u.Roles, new List<string> { Roles.User })
                    .RuleFor(u => u.Location, f => f.PickRandom(locations));

                List<User> fakeUsers = faker.Generate(SeedCount);

                // Random generate friendships
                var random = new Random();
                foreach (User fakeUser in fakeUsers)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        int index = random.Next(users.Count);
                        fakeUser.Friends.Add(users[index].Id);
                    }
                }

                users.AddRange(fakeUsers);
            }

            if (users.Any())
            {
                try
                {
                    await _unitOfWork.InsertManyAsync(users);
                }
                catch (DuplicateKeyException)
                {
                    // The tests are executed in parallel.
                    // Just ignore.
                }
            }
        }
    }
}