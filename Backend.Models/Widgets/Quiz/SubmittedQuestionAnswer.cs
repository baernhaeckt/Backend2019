using System.Collections.Generic;

namespace Backend.Models.Widgets.Quiz
{
    public class SubmittedQuestionAnswer
    {
        public QuestionResponse Question { get; set; }

        public IEnumerable<string> SelectedAnswers { get; set; }

        public string DetailedAnswer { get; set; }

        public int Points { get; set; }

        public bool IsCorrect { get; set; }
    }
}
