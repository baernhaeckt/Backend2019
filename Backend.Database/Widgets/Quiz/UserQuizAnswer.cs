using System.Collections.Generic;

namespace Backend.Database.Widgets.Quiz
{
    public class UserQuizAnswer
    {
        public string QuizQuestionId { get; set; }

        public IList<string> SelectedAnswer { get; set; }

        public bool IsCorrect { get; set; }

        public int Points { get; set; }
    }
}