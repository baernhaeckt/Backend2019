using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities.Quiz;
using Backend.Infrastructure.Hosting.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.Quiz.Data
{
    public class GenerateQuizQuestionsStartupTask : IStartupTask
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateQuizQuestionsStartupTask(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (await _unitOfWork.CountAsync<QuizQuestion>() != 0)
            {
                return;
            }

            IList<QuizQuestion> questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "Von 100 gesammelten Kartoffeln wieviele werden tatsächlich gegessen?",
                    CorrectAnswers = new[] { "34" },
                    IncorrectAnswers = new[] { "64", "50", "25", "10", "82" },
                    DetailedAnswer = "Zu krumm, zu klein, zu hässlich: Die Gründe für das Wegwerfen von Lebensmitteln sind unzählig, sinnvoll sind sie oft nicht. Beispielsweise werden von 100 geernteten Kartoffeln nur 34 tatsächlich gegessen. 66 Kartoffeln werden aussortiert, obwohl sie geniessbar wären. Foodwaste ist eine unnötige Verschwendung von Ressourcen, Energie und Geld. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste",

                    Points = 2
                },
                new QuizQuestion
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
                },
                new QuizQuestion
                {
                    Question = "Wie viele der Fischgründe weltweit sind überfischt?",
                    CorrectAnswers = new[] { "Mehr als 30 Prozent" },
                    IncorrectAnswers = new[] { "Mehr als 50 Prozent", "Mehr als 70 Prozent" },
                    DetailedAnswer = "Etwa 30 Prozent der kommerziell genutzten Fischbestände gelten als überfischt, 60 Prozent der Fischbestände sind maximal befischt. Nur 10 Prozent der globalen Fischbestände sind noch nicht an der Belastungsgrenze befischt.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Wer hat die Agenda 2030 für Nachhaltige Entwicklung beschlossen?",
                    CorrectAnswers = new[] { "Die 193 Mitgliedstaaten der Vereinten Nationen" },
                    IncorrectAnswers = new[] { "Ein Bündnis aus 190 Staaten - ohne USA und China", "Die europäische Union" },
                    DetailedAnswer = "Am 25. September 2015 haben die 193 Mitgliedsstaaten der UNO die Agenda 2030 für nachhaltige Entwicklung verabschiedet. Diese ist das Ergebnis aus der Zusammenführung der UNO-Konferenzen für nachhaltige Entwicklung 1992, 2002, 2012 und den Ende 2015 ausgelaufenen Millenniumsentwicklungszielen.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Wer beim Einkauf auf Nachhaltigkeit achtet der kauft am besten?",
                    CorrectAnswers = new[] { "bevorzugt regionale Produkte" },
                    IncorrectAnswers = new[] { "nicht im Supermarkt", "Bioprodukte" },
                    DetailedAnswer = "Wer nachhaltig einkaufen will, achtet beim Einkauf am besten immer auf regionale und saisonale Ware und auf eine biologische Produktion, denn gerade Importprodukte sind durch lange Transportwege umweltschädlich und nicht nachhaltig. Wer nicht auf Importware verzichten möchte, sollte auf Fairtrade-Siegel achten, denn diese garantieren eine faire und soziale Produktion.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Und welche Lichtquelle sorgt am nachhaltigsten für Licht?",
                    CorrectAnswers = new[] { "LED-Birnen" },
                    IncorrectAnswers = new[] { "Energiesparlampen", "Kerzen" },
                    DetailedAnswer = "LED verbrauchen weniger Strom als andere Leuchtmittel. Um bis zu 80 % können Unternehmen ihre Energiekosten für die Beleuchtung reduzieren, wenn sie auf moderne LED-Lichttechnik setzen.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Wie lange müsste man auf Fleisch verzichten, um einen Flug von Frankfurt nach Madrid wieder auszugleichen?",
                    CorrectAnswers = new[] { "Je nach Fleischart 1 - 2.5 Jahre" },
                    IncorrectAnswers = new[] { "Je nach Fleischart 9 - 16 Monate", "2 Monate" },
                    DetailedAnswer = "Um diesen Flug auszugleichen musst du auf 51kg Rind oder 160kg Geflügel verzichten.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Was ist bei der Jagd auf Garnelen und Plattfische ein Problem?",
                    CorrectAnswers = new[] { "Die Fangschiffe brauchen exorbitant viel Treibstoff." },
                    IncorrectAnswers = new[] { "Es werden zu 70 Prozent Jungtiere gefangen.", "Es fällt viel Beifang an." },
                    DetailedAnswer = "Bis zu 80 Prozent der gefangenen Tiere gehen als Beifang wieder über Bord, wobei viele Tiere tot oder schwer verletzt sind. Achten Sie bei Fisch auf das Siegel des Marine Stewardship Council (MSC), einer gemeinnützigen Organisation, die sich für nachhaltige Fischerei einsetzt.",
                    Points = 2
                },
                new QuizQuestion
                {
                    Question = "Was versteht man unter Greenwashing?",
                    CorrectAnswers = new[] { "grüne Augenwischerei" },
                    IncorrectAnswers = new[] { "nachhaltig produzierte Waschmaschinen", "besondere Autowaschanlagen" },
                    DetailedAnswer = "Natürlich haben die Hersteller erkannt, das sich mit dem Naturtrend gut verdienen lässt. Greenwashing kommt z.B. auch in der Kosmetik-Industrie vor, wo der Begriff Bio nicht geschützt ist. Etwas Bio-Öl in die Creme geben, schon wird das Ganze als Natur- oder Bio-Produkt verkauft. Achten Sie auch hier auf Prüfsiegel wie BDIH oder Eco Control.",
                    Points = 2
                }
            };

            await _unitOfWork.InsertManyAsync(questions);
        }
    }
}