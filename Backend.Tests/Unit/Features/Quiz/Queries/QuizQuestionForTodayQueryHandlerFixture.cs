using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Common;
using Backend.Core.Features.Quiz.Data;
using Backend.Core.Features.Quiz.Queries;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Hosting;
using Backend.Tests.Utilities;
using Divergic.Logging.Xunit;
using NSubstitute;
using Silverback.Messaging.Publishing;
using Xunit;
using Xunit.Abstractions;

namespace Backend.Tests.Unit.Features.Quiz.Queries
{
    public class QuizQuestionForTodayQueryHandlerFixture
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public QuizQuestionForTodayQueryHandlerFixture(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public async Task ExecuteAsync_ReturnsQuestion()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ICacheLogger<QuizQuestionForTodayQueryHandler> logger = _testOutputHelper.BuildLoggerFor<QuizQuestionForTodayQueryHandler>();
            IClock clock = new AdjustableClock();
            var quizQuestionForTodayQueryHandler = new QuizQuestionForTodayQueryHandler(inMemoryUnitOfWork, logger, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("20d05c0c-dd4d-46be-9951-c73a3de1b27d");

            // Act
            QuizQuestionForTodayQueryResult? result = await quizQuestionForTodayQueryHandler.ExecuteAsync(new QuizQuestionForTodayQuery(userId));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsNoQuestion_IfAlreadyAnsweredMax()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ICacheLogger<QuizQuestionForTodayQueryHandler> logger = _testOutputHelper.BuildLoggerFor<QuizQuestionForTodayQueryHandler>();
            IClock clock = new AdjustableClock();
            var quizQuestionForTodayQueryHandler = new QuizQuestionForTodayQueryHandler(inMemoryUnitOfWork, logger, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("11d05c0c-dd4d-46be-9951-c73a3de1b27d");

            var answerQuizQuestionCommandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>(), Substitute.For<IEventPublisher>(), clock);

            foreach (Question question in inMemoryUnitOfWork.Entities[typeof(Question)].Cast<Question>().Take(Constants.MaxQuestionsPerDay))
            {
                await answerQuizQuestionCommandHandler.ExecuteAsync(new AnswerQuizQuestionCommand(userId, question.Id, question.Answers.Single(a => a.IsCorrect).Id));
            }

            // Act
            QuizQuestionForTodayQueryResult? result = await quizQuestionForTodayQueryHandler.ExecuteAsync(new QuizQuestionForTodayQuery(userId));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsNewQuestionsOnNextDay_IfAllWereAnsweredForToday()
        {
            // Arrange
            var inMemoryUnitOfWork = new InMemoryUnitOfWork();
            ICacheLogger<QuizQuestionForTodayQueryHandler> logger = _testOutputHelper.BuildLoggerFor<QuizQuestionForTodayQueryHandler>();
            var clock = new AdjustableClock();
            var quizQuestionForTodayQueryHandler = new QuizQuestionForTodayQueryHandler(inMemoryUnitOfWork, logger, clock);

            var newGenerateQuizQuestionsStartupTask = new GenerateQuizQuestionsStartupTask(inMemoryUnitOfWork);
            await newGenerateQuizQuestionsStartupTask.ExecuteAsync(CancellationToken.None);

            var userId = new Guid("10c05c0c-dd4d-46be-9951-c73a3de1b27d");

            var answerQuizQuestionCommandHandler = new AnswerQuizQuestionCommandHandler(inMemoryUnitOfWork, _testOutputHelper.BuildLoggerFor<AnswerQuizQuestionCommandHandler>(), Substitute.For<IEventPublisher>(), clock);

            foreach (Question question in inMemoryUnitOfWork.Entities[typeof(Question)].Cast<Question>().Take(Constants.MaxQuestionsPerDay))
            {
                await answerQuizQuestionCommandHandler.ExecuteAsync(new AnswerQuizQuestionCommand(userId, question.Id, question.Answers.Single(a => a.IsCorrect).Id));
            }

            // Act
            QuizQuestionForTodayQueryResult? result1 = await quizQuestionForTodayQueryHandler.ExecuteAsync(new QuizQuestionForTodayQuery(userId));
            clock.GetNow = () => DateTimeOffset.UtcNow.AddDays(1);
            QuizQuestionForTodayQueryResult? result2 = await quizQuestionForTodayQueryHandler.ExecuteAsync(new QuizQuestionForTodayQuery(userId));

            // Assert
            Assert.Null(result1);
            Assert.NotNull(result2);
        }
    }
}