namespace Backend.Models.Widgets.Quiz
{
    public class QuestionAnswerResponse
    {
        public bool IsCorrect { get; set; }

        public int AwardedPoints { get; set; }

        public string DetailedAnswer { get; set; }
    }