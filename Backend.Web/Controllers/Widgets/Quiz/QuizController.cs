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
        public IQuizService QuizService { get; }

        public QuizController(IQuizService quizService)
        {
            QuizService = quizService;
        }

        [HttpGet("question")]
        public async Task<QuestionResponse> Get()
        {
            return await QuizService.Get();
        }

        [HttpPost("question")]
        public async Task<QuestionAnswerResponse> Answer(QuestionAnswer questionAnswer)
        {
            return await QuizService.Answer(questionAnswer);
        }

        [HttpGet("seed")]
        public void Seed()
        {
            QuizService.Insert(new Database.Widgets.Quiz.QuizQuestion()
            {
                Question = "Von 100 gesammelten Kartoffeln wieviele werden tatsächlich gegessen?",
                CorrectAnswers = new[] { "34" },
                IncorrectAnswers = new[] { "64", "50", "25", "10", "82" },
                DetailedAnswer = "Zu krumm, zu klein, zu hässlich: Die Gründe für das Wegwerfen von Lebensmitteln sind unzählig, sinnvoll sind sie oft nicht. Beispielsweise werden von 100 geernteten Kartoffeln nur 34 tatsächlich gegessen. 66 Kartoffeln werden aussortiert, obwohl sie geniessbar wären. Foodwaste ist eine unnötige Verschwendung von Ressourcen, Energie und Geld. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste",

                Points = 2
            });
            QuizService.Insert(new Database.Widgets.Quiz.QuizQuestion()
            {
                Question = "Wenn wir unseren Food Waste um einen Drittel reduzieren würde, was für einem CO2 Gewinn würde dies bedeuten?",
                CorrectAnswers = new[] { "gleichviel wie 500'000 Autos" },
                IncorrectAnswers = new[] { "gleichviel wie 10'000 Autos", "gleichviel wie 25'000 Autos", "gleichviel wie 2'500 Autos" },
                DetailedAnswer = @"Foodwaste muss nicht sein. Wenn alle Beteiligten es schaffen, mindestens einen Drittel der heutigen Lebensmittelverluste zu verhindern, könnten wir beispielsweise die Menge an CO2 einsparen, die 500'000 Autos verursachen.

                Zwar hat sich die Schweiz im Rahmen der «UN Sustainable Development Goals» zum Ziel bekannt,
                bis 2030 weltweit die Verluste von Lebensmitteln zu halbieren.Nur: An verbindlichen Zielvorgaben und konkreten Umsetzungsmassnahmen fehlt es weiterhin.Die WWF - Foodwaste - Petition von 2014,
                die genau dies für Industrie und Handel forderte,
                wurde von National - und Ständerat abgelehnt. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste",
                Points = 2
            });
        }
    }
}