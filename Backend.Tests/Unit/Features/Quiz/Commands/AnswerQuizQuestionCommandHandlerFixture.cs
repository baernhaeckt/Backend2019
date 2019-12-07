using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Events;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Data;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Tests.Utilities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Silverback.Messaging.Messages;
using Silverback.Messaging.Publishing;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Unit.Features.Quiz.Commands
{
    public class AnswerQuizQuestionCommandHandlerFixture
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AnswerQuizQuestionCommandHandlerFixture(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public async Task ExecuteAsync_CorrectAnswer_Successful()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<AnswerQuizQuestionCommandHandler> logger = _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>();
            IEventPublisher eventPublisher = Substitute.For<IEventPublisher>();
            IClock clock = new AdjustableClock();
            var commandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, logger, eventPublisher, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("10d05c0c-dd4d-46be-9951-c73a3de1b27d");

            // Act
            var command = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question1.Id, GenerateQuizQuestionsStartupTask.Question1.Answers.Single(a => a.IsCorrect).Id);
            AnswerQuizQuestionResult result = await commandHandler.ExecuteAsync(command);

            // Assert
            Assert.True(result.IsCorrect);
            await eventPublisher.Received().PublishAsync(Arg.Is<QuizQuestionCorrectAnsweredEvent>(e => e.UserId == userId && e.QuestionPoints == GenerateQuizQuestionsStartupTask.Question1.Points));
        }

        [Fact]
        public async Task ExecuteAsync_WrongAnswer_Successful()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<AnswerQuizQuestionCommandHandler> logger = _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>();
            IEventPublisher eventPublisher = Substitute.For<IEventPublisher>();
            IClock clock = new AdjustableClock();
            var commandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, logger, eventPublisher, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("a0d05c0c-dd4d-46be-9951-c73a3de1b27d");

            // Act
            var command = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question2.Id, GenerateQuizQuestionsStartupTask.Question2.Answers.First(a => !a.IsCorrect).Id);
            AnswerQuizQuestionResult result = await commandHandler.ExecuteAsync(command);

            // Assert
            Assert.False(result.IsCorrect);
            await eventPublisher.DidNotReceive().PublishAsync(Arg.Any<IEvent>());
        }

        [Fact]
        public async Task ExecuteAsync_AnswerTwice_Fails()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<AnswerQuizQuestionCommandHandler> logger = _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>();
            IEventPublisher eventPublisher = Substitute.For<IEventPublisher>();
            IClock clock = new AdjustableClock();
            var commandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, logger, eventPublisher, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("b0d05c0c-dd4d-46be-9951-c73a3de1b27d");

            var command = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question3.Id, GenerateQuizQuestionsStartupTask.Question3.Answers.First(a => !a.IsCorrect).Id);
            AnswerQuizQuestionResult result = await commandHandler.ExecuteAsync(command);

            // Act & Assert
            command = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question3.Id, GenerateQuizQuestionsStartupTask.Question3.Answers.First(a => !a.IsCorrect).Id);
            await Assert.ThrowsAsync<ValidationException>(() => commandHandler.ExecuteAsync(command));
            await eventPublisher.DidNotReceive().PublishAsync(Arg.Any<IEvent>());
        }

        [Fact]
        public async Task ExecuteAsync_AnswerMoreThanAllowedPerDay_Fails()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ILogger<AnswerQuizQuestionCommandHandler> logger = _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>();
            IEventPublisher eventPublisher = Substitute.For<IEventPublisher>();
            IClock clock = new AdjustableClock();
            var commandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, logger, eventPublisher, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("a1c14c0c-dd4d-46be-9951-c73a3de1b27d");

            // Act & Assert
            var command1 = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question4.Id, GenerateQuizQuestionsStartupTask.Question4.Answers.First(a => a.IsCorrect).Id);
            var command2 = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question5.Id, GenerateQuizQuestionsStartupTask.Question5.Answers.First(a => a.IsCorrect).Id);
            var command3 = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question6.Id, GenerateQuizQuestionsStartupTask.Question6.Answers.First(a => !a.IsCorrect).Id);
            var command4 = new AnswerQuizQuestionCommand(userId, GenerateQuizQuestionsStartupTask.Question7.Id, GenerateQuizQuestionsStartupTask.Question7.Answers.First(a => a.IsCorrect).Id);
            AnswerQuizQuestionResult result = await commandHandler.ExecuteAsync(command1);
            Assert.True(result.IsCorrect);
            result = await commandHandler.ExecuteAsync(command2);
            Assert.True(result.IsCorrect);
            result = await commandHandler.ExecuteAsync(command3);
            Assert.False(result.IsCorrect);

            await Assert.ThrowsAsync<ValidationException>(() => commandHandler.ExecuteAsync(command4));

            await eventPublisher.Received(2).PublishAsync(Arg.Any<IEvent>());
        }
    }
}
