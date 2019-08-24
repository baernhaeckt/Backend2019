namespace Backend.Database.Widgets.Quiz
{
    public class UserQuizAnswer
    {
        public string QuizQuestionId { get; set; }

        public string SelectedAnswer { get; set; }

        public bool IsCorrect { get; set; }

        public int Points { get; set; }
    }
}