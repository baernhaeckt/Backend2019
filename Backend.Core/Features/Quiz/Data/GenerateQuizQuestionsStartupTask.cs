using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Entities.Quiz;
using Backend.Infrastructure.Abstraction.Hosting;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Core.Features.Quiz.Data
{
    public class GenerateQuizQuestionsStartupTask : IStartupTask
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateQuizQuestionsStartupTask(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        internal static Question Question1 { get; } = new Question
        {
            Id = Guid.Parse("6a614844-3e66-4430-8faa-6016de35c4eb"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Von 100 gesammelten Kartoffeln wieviele werden tatsächlich gegessen?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Zu krumm, zu klein, zu hässlich: Die Gründe für das Wegwerfen von Lebensmitteln sind unzählig, sinnvoll sind sie oft nicht. Beispielsweise werden von 100 geernteten Kartoffeln nur 34 tatsächlich gegessen. 66 Kartoffeln werden aussortiert, obwohl sie geniessbar wären. Foodwaste ist eine unnötige Verschwendung von Ressourcen, Energie und Geld. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5a614844-3e66-4430-8faa-6016de35c4eb"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "34"),
                        new KeyValuePair<string, string>("en", "34"),
                        new KeyValuePair<string, string>("fr", "34"),
                        new KeyValuePair<string, string>("it", "34")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("f3ea2102-a459-4dea-a7f3-1dab34ae2cf9"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "64"),
                        new KeyValuePair<string, string>("en", "64"),
                        new KeyValuePair<string, string>("fr", "64"),
                        new KeyValuePair<string, string>("it", "64")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("6afc8bf9-d09d-4b79-8c17-cefb554a5b3c"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "50"),
                        new KeyValuePair<string, string>("en", "50"),
                        new KeyValuePair<string, string>("fr", "50"),
                        new KeyValuePair<string, string>("it", "50")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("c4edd985-b373-4c99-9290-914d70367fc7"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "25"),
                        new KeyValuePair<string, string>("en", "25"),
                        new KeyValuePair<string, string>("fr", "25"),
                        new KeyValuePair<string, string>("it", "25")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("62ec94da-ebcd-4bdb-883f-280d9ea34636"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "10"),
                        new KeyValuePair<string, string>("en", "10"),
                        new KeyValuePair<string, string>("fr", "10"),
                        new KeyValuePair<string, string>("it", "10")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("0561bdc4-cbdb-4406-a583-02712af79d92"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "82"),
                        new KeyValuePair<string, string>("en", "82"),
                        new KeyValuePair<string, string>("fr", "82"),
                        new KeyValuePair<string, string>("it", "82")
                    }
                }
            }
        };

        internal static Question Question2 { get; } = new Question
        {
            Id = Guid.Parse("6a614844-3e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wenn wir unseren Food Waste um einen Drittel reduzieren würde, was für einem CO2 Gewinn würde dies bedeuten?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Foodwaste muss nicht sein. Wenn alle Beteiligten es schaffen, mindestens einen Drittel der heutigen Lebensmittelverluste zu verhindern, könnten wir beispielsweise die Menge an CO2 einsparen, die 500'000 Autos verursachen. Zwar hat sich die Schweiz im Rahmen der «UN Sustainable Development Goals» zum Ziel bekannt, bis 2030 weltweit die Verluste von Lebensmitteln zu halbieren.Nur: An verbindlichen Zielvorgaben und konkreten Umsetzungsmassnahmen fehlt es weiterhin.Die WWF - Foodwaste - Petition von 2014, die genau dies für Industrie und Handel forderte, wurde von National - und Ständerat abgelehnt. Quelle: https://www.wwf.ch/de/unsere-ziele/foodwaste"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "gleichviel wie 500'000 Autos"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "gleichviel wie 10'000 Autos"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "gleichviel wie 25'000 Autos"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("dbb307fb-b286-4f52-bb89-7195da743a7e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "gleichviel wie 2'500 Autos"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question3 { get; } = new Question
        {
            Id = Guid.Parse("1a614844-3e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wie viele der Fischgründe weltweit sind überfischt?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Etwa 30 Prozent der kommerziell genutzten Fischbestände gelten als überfischt, 60 Prozent der Fischbestände sind maximal befischt. Nur 10 Prozent der globalen Fischbestände sind noch nicht an der Belastungsgrenze befischt."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Mehr als 30 Prozent"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Mehr als 50 Prozent"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Mehr als 70 Prozent"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question4 { get; } = new Question
        {
            Id = Guid.Parse("1a111544-3e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wer beim Einkauf auf Nachhaltigkeit achtet der kauft am besten?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wer nachhaltig einkaufen will, achtet beim Einkauf am besten immer auf regionale und saisonale Ware und auf eine biologische Produktion, denn gerade Importprodukte sind durch lange Transportwege umweltschädlich und nicht nachhaltig. Wer nicht auf Importware verzichten möchte, sollte auf Fairtrade-Siegel achten, denn diese garantieren eine faire und soziale Produktion."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "bevorzugt regionale Produkte"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "nicht im Supermarkt"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Bioprodukte"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question5 { get; } = new Question
        {
            Id = Guid.Parse("1bc24544-3e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wer hat die Agenda 2030 für Nachhaltige Entwicklung beschlossen?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Am 25. September 2015 haben die 193 Mitgliedsstaaten der UNO die Agenda 2030 für nachhaltige Entwicklung verabschiedet. Diese ist das Ergebnis aus der Zusammenführung der UNO-Konferenzen für nachhaltige Entwicklung 1992, 2002, 2012 und den Ende 2015 ausgelaufenen Millenniumsentwicklungszielen."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Die 193 Mitgliedstaaten der Vereinten Nationen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Ein Bündnis aus 190 Staaten - ohne USA und China"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Die europäische Union"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question6 { get; } = new Question
        {
            Id = Guid.Parse("2b245524-1e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Und welche Lichtquelle sorgt am nachhaltigsten für Licht?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "LED verbrauchen weniger Strom als andere Leuchtmittel. Um bis zu 80 % können Unternehmen ihre Energiekosten für die Beleuchtung reduzieren, wenn sie auf moderne LED-Lichttechnik setzen."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "LED-Birnen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Energiesparlampen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Kerzen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question7 { get; } = new Question
        {
            Id = Guid.Parse("5c133324-1e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Wie lange müsste man auf Fleisch verzichten, um einen Flug von Frankfurt nach Madrid wieder auszugleichen?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Um diesen Flug auszugleichen musst du auf 51kg Rind oder 160kg Geflügel verzichten."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Je nach Fleischart 1 - 2.5 Jahre"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Je nach Fleischart 9 - 16 Monate"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "2 Monate"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question8 { get; } = new Question
        {
            Id = Guid.Parse("5c133324-1e66-4430-8faa-1016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Was ist bei der Jagd auf Garnelen und Plattfische ein Problem?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Bis zu 80 Prozent der gefangenen Tiere gehen als Beifang wieder über Bord, wobei viele Tiere tot oder schwer verletzt sind. Achten Sie bei Fisch auf das Siegel des Marine Stewardship Council (MSC), einer gemeinnützigen Organisation, die sich für nachhaltige Fischerei einsetzt."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Es fällt viel Beifang an."),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Es werden zu 70 Prozent Jungtiere gefangen."),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "Die Fangschiffe brauchen exorbitant viel Treibstoff."),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        internal static Question Question9 { get; } = new Question
        {
            Id = Guid.Parse("11111111-1e66-4430-8faa-6016de35c4ec"),
            Points = 2,
            QuestionText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Was versteht man unter Greenwashing?"),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            ExplanationText = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Natürlich haben die Hersteller erkannt, das sich mit dem Naturtrend gut verdienen lässt. Greenwashing kommt z.B. auch in der Kosmetik-Industrie vor, wo der Begriff Bio nicht geschützt ist. Etwas Bio-Öl in die Creme geben, schon wird das Ganze als Natur- oder Bio-Produkt verkauft. Achten Sie auch hier auf Prüfsiegel wie BDIH oder Eco Control."),
                new KeyValuePair<string, string>("en", "en"),
                new KeyValuePair<string, string>("fr", "en"),
                new KeyValuePair<string, string>("it", "en")
            },
            Answers = new List<Answer>
            {
                new Answer
                {
                    Id = Guid.Parse("5f30bc0c-9af3-428a-8435-5d23d1982e46"),
                    IsCorrect = true,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "grüne Augenwischerei"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("d72a1d2e-0510-4934-9926-78a64676b74e"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "nachhaltig produzierte Waschmaschinen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                },
                new Answer
                {
                    Id = Guid.Parse("fddb8936-0f69-4d91-aa99-21d843fe29cf"),
                    IsCorrect = false,
                    AnswerText = new LocalizedField
                    {
                        new KeyValuePair<string, string>("de", "besondere Autowaschanlagen"),
                        new KeyValuePair<string, string>("en", "en"),
                        new KeyValuePair<string, string>("fr", "fr"),
                        new KeyValuePair<string, string>("it", "it")
                    }
                }
            }
        };

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (await _unitOfWork.CountAsync<Question>() != 0)
            {
                return;
            }

            IList<Question> questions = new List<Question>
            {
                Question1,
                Question2,
                Question3,
                Question4,
                Question5,
                Question6,
                Question7,
                Question8,
                Question9
            };

            await _unitOfWork.InsertManyAsync(questions);
        }
    }
}