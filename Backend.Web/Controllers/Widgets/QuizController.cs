using Backend.Core.Services.Widgets;
using Backend.Models.Widgets.Quiz;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Web.Controllers.Widgets
{
    [Route("api/widgets/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("question")]
        public async Task<QuestionResponse> Get() => await _quizService.Get();

        [HttpPost("question")]
        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer questionAnswer) => await _quizService.Answer(questionAnswer);
    }
}