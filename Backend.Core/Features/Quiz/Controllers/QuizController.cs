using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Core.Features.Quiz.Models;

namespace Backend.Core.Features.Quiz.Controllers
{
    [Route("api/widgets/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("question")]
        public async Task<QuestionResponse> Get() => await _quizService.Get();

        [HttpPost("question")]
        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer questionAnswer) => await _quizService.Answer(questionAnswer);
    }
}