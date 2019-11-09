using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Abstraction.Persistence;
using Silverback.Messaging.Subscribers;

namespace Backend.Core.Features.Partner.Commands
{
    public class CreateNewTokenCommandHandler : ISubscriber
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNewTokenCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Guid> ExecuteAsync(CreateNewTokenCommand command)
        {
            TokenIssuer tokenIssuer = await _unitOfWork.GetByIdOrThrowAsync<TokenIssuer>(command.PartnerId);
            Token tokenPrototype = tokenIssuer.PrototypeTokens.SingleOrDefault(p => p.TokenType == command.TokenType.ToLowerInvariant());
            if (tokenPrototype == null)
            {
                throw new EntityNotFoundException(typeof(Token), nameof(ExecuteAsync), "Issuer: " + tokenIssuer.Id + "TokenType: " + command.TokenType);
            }

            Token newToken = tokenPrototype.CreateFromPrototype();
            newToken.PartnerId = tokenIssuer.Id;
            await _unitOfWork.InsertAsync(newToken);
            return newToken.Id;
        }
    }
}