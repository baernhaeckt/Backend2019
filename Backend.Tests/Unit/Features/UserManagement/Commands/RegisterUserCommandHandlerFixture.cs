using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Infrastructure.Abstraction.Security;
using Backend.Tests.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Silverback.Messaging.Publishing;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Unit.Features.UserManagement.Commands
{
    public class RegisterUserCommandHandlerFixture
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RegisterUserCommandHandlerFixture(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public async Task ExecuteAsync_ThrowsValidationException_IfEmailAddressIsInvalid()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RegisterUserCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RegisterUserCommandHandler>();
            var eventPublisher = Substitute.For<IEventPublisher>();
            var passwordGenerator = Substitute.For<IPasswordGenerator>();
            var passwordStorage = Substitute.For<IPasswordStorage>();
            var commandHandler = new RegisterUserCommandHandler(inMemoryUnitOfWork, logger, passwordGenerator, passwordStorage, eventPublisher);

            // Act & Assert
            var command = new RegisterUserCommand("a@");
            await Assert.ThrowsAsync<ValidationException>(() => commandHandler.ExecuteAsync(command));
        }
    }
}