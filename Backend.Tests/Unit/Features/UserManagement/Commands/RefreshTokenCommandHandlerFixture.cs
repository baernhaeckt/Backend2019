using System;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Features.UserManagement.Commands;
using Backend.Core.Features.UserManagement.Shared;
using Backend.Tests.Utilities;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Unit.Features.UserManagement.Commands
{
    public class RefreshTokenCommandHandlerFixture
    {
        private const string RefreshTokenValueHash = "UTiDyt8AllpjUAN9yZo8WmriQScaINhdYgfUTvOm3QE=";

        private const string RefreshTokenValueHashValid = "P5yrLH3V5gEdtJiQETNNJAers78yeIlSCXDK3PuU35Q=";

        private const string RefreshTokenValue = "9zvDKDt1dfS3rc7RVV5ei1LFiFEzRYCEDQNA/6H5mb0c6/re4hYrKKVGxwSSq95pbGrM8uCNVZzRSV9WNUS7mOP94pDIBs6Q91aQWTXSIUWGDnhFfdkSVxC+k3pvxBkr4gj/DywtIegL5DZ3lnSCXTGwKFR5jbsSN4uFq3mhrqX0o6uuxT8ku4SO4HgRaicDmFYK72CB0QS6nSNTfRpucpz7kzfTKr5Ik5rPxRQACPyDzF2lQfEJ/N+4bT7hPCenXMlHQczQkFuXIfzR+GP+iY9ScfcA8F/WezV7aRxey6MhThjn9aGo5aSj8f3I5CTkrpn32ZnrbE5Alu/uEvDQaQ==";

        private readonly ITestOutputHelper _testOutputHelper;

        public RefreshTokenCommandHandlerFixture(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public async Task ExecuteAsync_SuccessfulRefreshToken()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RefreshTokenCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RefreshTokenCommandHandler>();
            await inMemoryUnitOfWork.InsertAsync(new RefreshToken { ExpiresAt = DateTimeOffset.Now.AddDays(1), UserId = Guid.Parse("C839007B-F34A-4F04-ADD9-26A29CB3E426"), ValueHash = RefreshTokenValueHashValid });
            var clock = new AdjustableClock();
            var commandHandler = new RefreshTokenCommandHandler(inMemoryUnitOfWork, logger, clock, new RefreshTokenStorage(inMemoryUnitOfWork, clock));

            // Act & Assert
            var command = new RefreshTokenCommand(RefreshTokenValue);
            RefreshTokenCommandResult result = await commandHandler.ExecuteAsync(command);

            Assert.False(result.IsExpired);
            Assert.False(result.IsAlreadyUsed);
            Assert.False(result.IsRevoked);
            Assert.False(result.TokenNotFound);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task ExecuteAsync_RevokesToken_IfExpired()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RefreshTokenCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RefreshTokenCommandHandler>();
            await inMemoryUnitOfWork.InsertAsync(new RefreshToken { ExpiresAt = DateTimeOffset.Now.AddDays(-1), UserId = Guid.Parse("C839007B-F34A-4F04-ADD9-26A29CB3E426"), ValueHash = RefreshTokenValueHashValid });
            var clock = new AdjustableClock();
            var commandHandler = new RefreshTokenCommandHandler(inMemoryUnitOfWork, logger, clock, new RefreshTokenStorage(inMemoryUnitOfWork, clock));

            // Act & Assert
            var command = new RefreshTokenCommand(RefreshTokenValue);
            RefreshTokenCommandResult result = await commandHandler.ExecuteAsync(command);

            Assert.True(result.IsExpired);
            Assert.False(result.IsAlreadyUsed);
            Assert.False(result.IsRevoked);
            Assert.False(result.TokenNotFound);
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task ExecuteAsync_RejectToken_IfRevoked()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RefreshTokenCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RefreshTokenCommandHandler>();
            await inMemoryUnitOfWork.InsertAsync(new RefreshToken { ExpiresAt = DateTimeOffset.Now.AddDays(1), UserId = Guid.Parse("C839007B-F34A-4F04-ADD9-26A29CB3E426"), IsRevoked = true, ValueHash = RefreshTokenValueHashValid });
            var clock = new AdjustableClock();
            var commandHandler = new RefreshTokenCommandHandler(inMemoryUnitOfWork, logger, clock, new RefreshTokenStorage(inMemoryUnitOfWork, clock));

            // Act & Assert
            var command = new RefreshTokenCommand(RefreshTokenValue);
            RefreshTokenCommandResult result = await commandHandler.ExecuteAsync(command);

            Assert.False(result.IsExpired);
            Assert.False(result.IsAlreadyUsed);
            Assert.True(result.IsRevoked);
            Assert.False(result.TokenNotFound);
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task ExecuteAsync_RejectToken_IfAlreadyUsed()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RefreshTokenCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RefreshTokenCommandHandler>();
            await inMemoryUnitOfWork.InsertAsync(new RefreshToken { ExpiresAt = DateTimeOffset.Now.AddDays(1), UserId = Guid.Parse("C839007B-F34A-4F04-ADD9-26A29CB3E426"), IsUsed = true, ValueHash = RefreshTokenValueHashValid });
            var clock = new AdjustableClock();
            var commandHandler = new RefreshTokenCommandHandler(inMemoryUnitOfWork, logger, clock, new RefreshTokenStorage(inMemoryUnitOfWork, clock));

            // Act & Assert
            var command = new RefreshTokenCommand(RefreshTokenValue);
            RefreshTokenCommandResult result = await commandHandler.ExecuteAsync(command);

            Assert.False(result.IsExpired);
            Assert.True(result.IsAlreadyUsed);
            Assert.False(result.IsRevoked);
            Assert.False(result.TokenNotFound);
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task ExecuteAsync_RejectToken_IfTokenNotFound()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<RefreshTokenCommandHandler> logger = _testOutputHelper.BuildLoggerFor<RefreshTokenCommandHandler>();
            await inMemoryUnitOfWork.InsertAsync(new RefreshToken { ExpiresAt = DateTimeOffset.Now.AddDays(1), UserId = Guid.Parse("C839007B-F34A-4F04-ADD9-26A29CB3E426"), ValueHash = RefreshTokenValueHash });
            var clock = new AdjustableClock();
            var commandHandler = new RefreshTokenCommandHandler(inMemoryUnitOfWork, logger, clock, new RefreshTokenStorage(inMemoryUnitOfWork, clock));

            // Act & Assert
            var command = new RefreshTokenCommand(RefreshTokenValue);
            RefreshTokenCommandResult result = await commandHandler.ExecuteAsync(command);

            Assert.False(result.IsExpired);
            Assert.False(result.IsAlreadyUsed);
            Assert.False(result.IsRevoked);
            Assert.True(result.TokenNotFound);
            Assert.Null(result.Token);
        }
    }
}