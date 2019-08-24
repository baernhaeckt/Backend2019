using AspNetCore.MongoDB;
using Backend.Core.Security.Abstraction;
using Backend.Database;
using Bogus;
using Bogus.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SeedController : ControllerBase
    {
        private readonly IPasswordStorage _passwordStorage;
        private readonly IMongoOperation<User> _operation;
        private readonly IPaswordGenerator _paswordGenerator;

        public SeedController(IPasswordStorage passwordStorage, IMongoOperation<User> operation, IPaswordGenerator paswordGenerator)
        {
            _passwordStorage = passwordStorage;
            _operation = operation;
            _paswordGenerator = paswordGenerator;
        }

        [HttpPost("users")]
        public void SeedUsers(int count)
        {
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

            List<User> users = faker.Generate(count);
            _operation.InsertMany(users);
        }
    }
}