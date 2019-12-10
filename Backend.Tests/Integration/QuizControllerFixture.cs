using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Data;
using Backend.Core.Features.Quiz.Models;
using Backend.Core.Features.Quiz.Queries;
using Backend.Tests.Utilities.Extensions;
using Xunit;

namespace Backend.Tests.Integration
{
    [Collection("IntegrationTests")]
    [Trait("Category", "Integration")]
    public class QuizControllerFixture : IClassFixture<TestContext>
    {
        private readonly TestContext _context;

        public QuizControllerFixture(TestContext context) => _context = context;

        [Fact]
        public async Task Get_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();
            var uri = new Uri("api/quiz", UriKind.Relative);
            HttpResponseMessage response = await _context.NewTestUserHttpClient.GetAsync(uri);
            QuizQuestionForTodayQueryResult questionResponse = await response.OnSuccessDeserialize<QuizQuestionForTodayQueryResult>();

            Assert.NotNull(questionResponse);
            Assert.NotNull(questionResponse.Question);
            Assert.NotNull(questionResponse.Answers);
        }

        [Fact]
        public async Task Answer_GiveCorrectAnswer_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();
            var uri = new Uri("api/quiz", UriKind.Relative);

            StringContent content = new QuestionAnswerRequest
            {
                QuestionId = GenerateQuizQuestionsStartupTask.Question1.Id,
                AnswerId = GenerateQuizQuestionsStartupTask.Question1.Answers.Single(a => a.IsCorrect).Id
            }.ToStringContent();

            HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(uri, content);
            AnswerQuizQuestionResult result = await response.OnSuccessDeserialize<AnswerQuizQuestionResult>();
            Assert.True(result.IsCorrect);
        }

        [Fact]
        public async Task Answer_GiveWrongAnswer_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();
            var uri = new Uri("api/quiz", UriKind.Relative);

            StringContent content = new QuestionAnswerRequest
            {
                QuestionId = GenerateQuizQuestionsStartupTask.Question2.Id,
                AnswerId = GenerateQuizQuestionsStartupTask.Question2.Answers.First(a => !a.IsCorrect).Id
            }.ToStringContent();

            HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(uri, content);
            AnswerQuizQuestionResult result = await response.OnSuccessDeserialize<AnswerQuizQuestionResult>();
            Assert.False(result.IsCorrect);
        }

        [Fact]
        public async Task Answer_CannotAnswerTwoTimes_Successful()
        {
            _context.NewTestUser = await _context.NewTestUserHttpClient.CreateUserAndSignIn();
            var uri = new Uri("api/quiz", UriKind.Relative);

            StringContent content = new QuestionAnswerRequest
            {
                QuestionId = GenerateQuizQuestionsStartupTask.Question3.Id,
                AnswerId = GenerateQuizQuestionsStartupTask.Question3.Answers.First(a => !a.IsCorrect).Id
            }.ToStringContent();

            HttpResponseMessage response = await _context.NewTestUserHttpClient.PostAsync(uri, content);
            AnswerQuizQuestionResult result = await response.OnSuccessDeserialize<AnswerQuizQuestionResult>();
            Assert.False(result.IsCorrect);

            response = await _context.NewTestUserHttpClient.PostAsync(uri, content);
            response.EnsureNotSuccessStatusCode();
        }
    }
}