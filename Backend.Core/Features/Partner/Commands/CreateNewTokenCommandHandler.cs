using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Partner.Commands
{
    public class CreateNewTokenCommandHandler : CommandHandlerWithReturnValue<CreateNewTokenCommand, Guid>
    {
        public CreateNewTokenCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateNewTokenCommandHandler> logger)
            : base(unitOfWork, logger)
        {
        }

        public override async Task<Guid> ExecuteAsync(CreateNewTokenCommand command)
        {
            Logger.InitiateCreateNewToken(command.PartnerId, command.TokenType);

            TokenIssuer tokenIssuer = await UnitOfWork.GetByIdOrThrowAsync<TokenIssuer>(command.PartnerId);
            Token tokenPrototype = tokenIssuer.PrototypeTokens.SingleOrDefault(p => p.TokenType == command.TokenType.ToLowerInvariant());
            if (tokenPrototype == null)
            {
                throw new EntityNotFoundException(typeof(Token), nameof(ExecuteAsync), "Issuer: " + tokenIssuer.Id + "TokenType: " + command.TokenType);
            }

            Token newToken = tokenPrototype.CreateFromPrototype();
            newToken.PartnerId = tokenIssuer.Id;
            await UnitOfWork.InsertAsync(newToken);

            Logger.CreateNewTokenSuccessful(command.PartnerId, command.TokenType, newToken.Id);

            return newToken.Id;
        }
    }
}