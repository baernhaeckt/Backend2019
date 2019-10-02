using System.Threading.Tasks;
using Backend.Core.Features.Quiz.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Quiz.Controllers
{
    [Route("api/widgets/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService) => _quizService = quizService;

        [HttpGet("question")]
        public async Task<QuestionResponse> Get()
        {
            return await _quizService.Get();
        }

        [HttpPost("question")]
        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer questionAnswer)
        {
            return await _quizService.Answer(questionAnswer);
        }
    }
}