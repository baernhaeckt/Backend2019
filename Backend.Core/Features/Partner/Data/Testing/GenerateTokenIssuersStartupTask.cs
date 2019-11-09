using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
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
                            Text = "Einkauf in der Palette",
                            Id = Guid.Empty,
                            TokenType = "verpackung",
                            IsSingleUse = true,
                            SufficientType = new SufficientType { Title = "Verpackung", Description = "Du hast Verpackungslos eingekauft." }
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
                            Text = "Nachbar über Suffizienz aufgeklärt",
                            Id = Guid.Empty,
                            TokenType = "wissen",
                            IsSingleUse = true,
                            SufficientType = new SufficientType { Title = "Wissen", Description = "Du hast dein Suffizienz mit anderen geteilt." }
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
                            Text = "Du Teilst deine Auto mit deiner Schwester",
                            Id = Guid.Empty,
                            TokenType = "teilen",
                            IsSingleUse = true,
                            SufficientType = new SufficientType { Title = "Teilen", Description = "Du hast deinen Besitz mit anderen geteilt." }
                        },
                        new Token
                        {
                            Points = 10,
                            Co2Saving = 0.1,
                            Text = "Bla",
                            Id = Guid.Empty,
                            TokenType = "multiuse",
                            IsSingleUse = false,
                            SufficientType = new SufficientType { Title = "X", Description = "Y." }
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