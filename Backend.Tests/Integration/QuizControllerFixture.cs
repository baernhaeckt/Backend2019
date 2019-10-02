using System;
using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Features.Quiz.Models;
using Backend.Tests.Integration.Utilities.Extensions;
using Xunit;

namespace Backend.Tests.Integration
{
    [Trait("Category", "Integration")]
    public class QuizControllerFixture : IClassFixture<TestContext>
    {
        private readonly TestContext _context;

        public QuizControllerFixture(TestContext context) => _context = context;

        [Fact]
        public async Task GetAndAnswer_Successful()
        {
            _context.NewTestUser = await _context.NewUserHttpClient.CreateUserAndSignIn();
            var uri = new Uri("api/quiz", UriKind.Relative);
            HttpResponseMessage response = await _context.NewUserHttpClient.GetAsync(uri);
            QuestionResponse questionResponse = await response.OnSuccessDeserialize<QuestionResponse>();

            StringContent content = new QuestionAnswer
            {
                QuestionId = questionResponse.Id,
                Answers = questionResponse.Answers
            }.ToStringContent();

            response = await _context.NewUserHttpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
    }
}