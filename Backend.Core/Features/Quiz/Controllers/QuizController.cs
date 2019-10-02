using System.Threading.Tasks;
using Backend.Core.Features.Quiz.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Features.Quiz.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService) => _quizService = quizService;

        [HttpGet]
        public async Task<QuestionResponse> Get() => await _quizService.Get();

        [HttpPost]
        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer questionAnswer) => await _quizService.Answer(questionAnswer);
    }
}