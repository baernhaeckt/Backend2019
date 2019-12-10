using System.Collections.Generic;

namespace Backend.Core.Entities
{
    public class QuizAnswerPointAction : PointAction
    {
        public QuizAnswerPointAction()
        {
        }

        public QuizAnswerPointAction(int points)
        {
            Action = new LocalizedField
            {
                new KeyValuePair<string, string>("de", "Hat eine korrekte Quizantwort gegeben."),
                new KeyValuePair<string, string>("en", "Has given a correct quiz answer."),
                new KeyValuePair<string, string>("it", "Ha dato una risposta corretta al quiz."),
                new KeyValuePair<string, string>("fr", "A donné une bonne réponse au questionnaire.")
            }.GetForCurrentCulture();

            Co2Saving = 0.0;
            Point = points;
            Type = SufficientType.Knowledge;
        }
    }
}