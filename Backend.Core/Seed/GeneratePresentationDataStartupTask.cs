using AspNetCore.MongoDB;
using Backend.Core.Security.Abstraction;
using Backend.Core.Startup;
using Backend.Database;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Core.Seed
{
    public class GeneratePresentationDataStartupTask : IStartupTask
    {
        public GeneratePresentationDataStartupTask(
            IMongoOperation<User> userRepository,
            IPasswordGenerator paswordGenerator,
            IMongoOperation<Token> tokenRepository)
        {
            UserRepository = userRepository;
            PaswordGenerator = paswordGenerator;
            TokenRepository = tokenRepository;
        }

        public IMongoOperation<Token> TokenRepository { get; }
        public IMongoOperation<User> UserRepository { get; }
        public IPasswordGenerator PaswordGenerator { get; }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var greta = UserRepository.GetQuerableAsync().FirstOrDefault(u => u.Email == "greta@bfh.ch");
            if (greta != null)
            {
                return;
            }

            await UserRepository.SaveAsync(new User()
            {
                DisplayName = "Greta",
                Email = "greta@bfh.ch",
                Password = PaswordGenerator.Generate(),
                Location = new Location
                {
                    City = "Bern",
                    Zip = "3011",
                    Latitude = 46.944699,
                    Longitude = 7.443788
                },
                Co2Saving = 140,
                PointActions = new[]
                {
                    new PointAction()
                    {
                        Date = new DateTime(2019, 08, 23, 10, 35, 00),
                        Co2Saving = 4,
                        SufficientType = new UserSufficientType
                        {
                            Title = "Unterstützen",
                            Co2Saving = 4,
                            Point = 35
                        },
                        Point = 35,
                        Action = "Vortrag zum Thema Klimaerwärmung gehalten"
                    },
                    new PointAction()
                    {
                        Date = new DateTime(2019, 08, 23, 16, 35, 00),
                        Co2Saving = 10,
                        SufficientType = new UserSufficientType
                        {
                            Title = "Energie",
                            Co2Saving = 10,
                            Point = 4
                        },
                        Point = 4,
                        Action = "Auf BKW Grüner Strom umgestellt."
                    },
                    new PointAction()
                    {
                        Date = new DateTime(2019, 08, 23, 17, 35, 00),
                        Co2Saving = 10,
                        SufficientType = new UserSufficientType
                        {
                            Title = "Teilen",
                            Co2Saving = 10,
                            Point = 4
                        },
                        Point = 4,
                        Action = "Eine Freundin auf Leaf eingeladen."
                    },
                    new PointAction()
                    {
                        Date = new DateTime(2019, 08, 24, 09, 13, 00),
                        Co2Saving = 10,
                        SufficientType = new UserSufficientType
                        {
                            Title = "Verpackung",
                            Co2Saving = 10,
                            Point = 13
                        },
                        Point = 13,
                        Action = "Ohne Verpackung bei Palette-Bern eingekauft."
                    },
                    new PointAction()
                    {
                        Date = new DateTime(2019, 08, 25, 09, 30, 00),
                        Co2Saving = 10,
                        SufficientType = new UserSufficientType
                        {
                            Title = "Wissen",
                            Co2Saving = 10,
                            Point = 20
                        },
                        Point = 20,
                        Action = "Ein Buch zum Thema Nachhaltigkeit gelesen"
                    }
                }
            });

            await TokenRepository.SaveAsync(new Token
            {
                Partner = "Leaf",
                Points = 100,
                Co2Saving = 0.1,
                CreatedDate = DateTime.Now,
                Text = "Auf der Startseite mit dem QR Code registrieret",
                Value = new Guid("9cceec7e-5c3c-488b-9fb6-c1aec5dc0923"),
                SufficientType = new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt."
                }
            });
            await TokenRepository.SaveAsync(new Token
            {
                Partner = "Leaf",
                Points = 80,
                Co2Saving = 0.1,
                CreatedDate = DateTime.Now,
                Text = "Auf der zweiten Seite mit dem QR Code registrieret",
                Value = new Guid("9e99f183-fb78-461c-af05-9d53fc75818e"),
                SufficientType = new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt."
                }
            });
            await TokenRepository.SaveAsync(new Token
            {
                Partner = "Leaf",
                Points = 55,
                Co2Saving = 0.1,
                CreatedDate = DateTime.Now,
                Text = "Beim BIO Bauer eingekauft.",
                Value = new Guid("81c83f19-3072-4d6d-8b70-617e10261dba"),
                SufficientType = new SufficientType
                {
                    Title = "Food Waste",
                    Description = "Du hast grün eingekauft."
                }
            });
        }
    }
}
