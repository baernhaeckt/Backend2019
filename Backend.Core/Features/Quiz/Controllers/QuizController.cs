using System.Threading.Tasks;
using Backend.Core.Extensions;
using Backend.Core.Features.Quiz.Commands;
using Backend.Core.Features.Quiz.Models;
using Backend.Core.Features.Quiz.Queries;
using Microsoft.AspNetCore.Mvc;
using Silverback.Messaging.Publishing;

namespace Backend.Core.Features.Quiz.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;

        private readonly IQueryPublisher _queryPublisher;

        public QuizController(ICommandPublisher commandPublisher, IQueryPublisher queryPublisher)
        {
            _commandPublisher = commandPublisher;
            _queryPublisher = queryPublisher;
        }

        [HttpGet]
        public async Task<QuizQuestionForTodayQueryResult> Get() => await _queryPublisher.ExecuteAsync(new QuizQuestionForTodayQuery(User.Id()));

        [HttpPost]
        public async Task<AnswerQuizQuestionResult> Answer(QuestionAnswer questionAnswer) => await _commandPublisher.ExecuteAsync(new AnswerQuizQuestionCommand(User.Id(), questionAnswer.QuestionId, questionAnswer.AnswerId));
    }
}