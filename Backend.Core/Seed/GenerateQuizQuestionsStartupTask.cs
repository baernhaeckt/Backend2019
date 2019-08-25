using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Core.Startup;
using Backend.Database.Widgets.Quiz;
using MongoDB.Driver;

namespace Backend.Core.Seed
{
    public class GenerateQuizQuestionsStartupTask : IStartupTask
    {
        private readonly IMongoOperation<QuizQuestion> _quizQuestionRepository;

        public GenerateQuizQuestionsStartupTask(IMongoOperation<QuizQuestion> quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_quizQuestionRepository.Count(FilterDefinition<QuizQuestion>.Empty) != 0)
            {
                return;
            }

            IList<QuizQuestion> questions = new List<QuizQuestion>();

            questions.Add(new QuizQuestion
            {
                Question = "Von 100 gesammelten Kartoffeln wieviele werden tatsächlich gegessen?",
                CorrectAnswers = new[] { "34" },
                IncorrectAnswers = new[] { "64", "50", "25", "10", "82" },
                DetailedAnswer = "Zu krumm, zu klein, zu hässlich: Die Gründe für das Wegwerfen von Lebensmitteln sind unzählig, sinnvoll sind sie oft nicht. Beispielsweise werden von 100 geernteten Kartoffeln nur 34 tatsächlich gegessen. 66 Kartoffeln werden aussortiert, obwohl sie geniessbar wären. Foodwaste ist eine unnötige Verschwendung von Ressourcen, Energie und Geld. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste",

                Points = 2
            });

            questions.Add(new QuizQuestion
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

            await _quizQuestionRepository.InsertManyAsync(questions);
        }
    }
}