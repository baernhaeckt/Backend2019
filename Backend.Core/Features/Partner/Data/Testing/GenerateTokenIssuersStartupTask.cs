using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Partner;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;
using Backend.Infrastructure.Abstraction.Security;

namespace Backend.Core.Features.Partner.Data.Testing
{
    public class GenerateTokenIssuersStartupTask : IStartupTask
    {
        private readonly IPasswordStorage _passwordStorage;

        private readonly IUnitOfWork _unitOfWork;

        public GenerateTokenIssuersStartupTask(IPasswordStorage passwordStorage, IUnitOfWork unitOfWork)
        {
            _passwordStorage = passwordStorage;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var tokenIssuers = new List<TokenIssuer>();

            if (await _unitOfWork.CountAsync<TokenIssuer>() == 0)
            {
                tokenIssuers.Add(new TokenIssuer
                {
                    Id = TokenIssuerTestCredentials.Id1,
                    SecretHash = _passwordStorage.Create("1234"),
                    Name = "Palette",
                    PrototypeTokens = new List<Token>
                    {
                        new Token
                        {
                            Points = 10,
                            Co2Saving = 1,
                            Text = new LocalizedField
                            {
                                new KeyValuePair<string, string>("de", "Einkauf in der Palette"),
                                new KeyValuePair<string, string>("en", "Purchasing in Palette"),
                                new KeyValuePair<string, string>("it", "Acquisto in Palette"),
                                new KeyValuePair<string, string>("fr", "Achats en Palette")
                            },
                            Id = Guid.Empty,
                            TokenType = "verpackung",
                            IsSingleUse = true,
                            SufficientType = SufficientType.Packing
                        }
                    }
                });

                tokenIssuers.Add(new TokenIssuer
                {
                    Id = TokenIssuerTestCredentials.Id2,
                    SecretHash = _passwordStorage.Create("1234"),
                    Name = "Dein Nachbar",
                    PrototypeTokens = new List<Token>
                    {
                        new Token
                        {
                            Points = 5,
                            Co2Saving = 0.1,
                            Text = new LocalizedField
                            {
                                new KeyValuePair<string, string>("de", "Nachbar über Suffizienz aufgeklärt"),
                                new KeyValuePair<string, string>("en", "Neighbour informed about sufficiency"),
                                new KeyValuePair<string, string>("it", "Vicino informato sulla sufficienza"),
                                new KeyValuePair<string, string>("fr", "Voisin informé de la suffisance")
                            },
                            Id = Guid.Empty,
                            TokenType = "wissen",
                            IsSingleUse = true,
                            SufficientType = SufficientType.Knowledge
                        }
                    }
                });

                tokenIssuers.Add(new TokenIssuer
                {
                    Id = TokenIssuerTestCredentials.Id3,
                    SecretHash = _passwordStorage.Create("1234"),
                    Name = "Meine Schwester",
                    PrototypeTokens = new List<Token>
                    {
                        new Token
                        {
                            Points = 15,
                            Co2Saving = 2,
                            Text = new LocalizedField
                            {
                                new KeyValuePair<string, string>("de", "Du Teilst deine Auto mit deiner Schwester"),
                                new KeyValuePair<string, string>("en", "You share your car with your sister"),
                                new KeyValuePair<string, string>("it", "Condividi la macchina con tua sorella"),
                                new KeyValuePair<string, string>("fr", "Tu partages ta voiture avec ta soeur")
                            },
                            Id = Guid.Empty,
                            TokenType = "teilen",
                            IsSingleUse = true,
                            SufficientType = SufficientType.Share
                        },
                        new Token
                        {
                            Points = 10,
                            Co2Saving = 1,
                            Text = new LocalizedField
                            {
                                new KeyValuePair<string, string>("de", "Nur für testing"),
                                new KeyValuePair<string, string>("en", "Only for testing"),
                                new KeyValuePair<string, string>("it", "Solo per i test"),
                                new KeyValuePair<string, string>("fr", "Uniquement pour les tests")
                            },
                            Id = Guid.Empty,
                            TokenType = "multiuse",
                            IsSingleUse = false,
                            SufficientType = SufficientType.FoodWaste
                        }
                    }
                });

                if (tokenIssuers.Any())
                {
                    try
                    {
                        await _unitOfWork.InsertManyAsync(tokenIssuers);
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
}