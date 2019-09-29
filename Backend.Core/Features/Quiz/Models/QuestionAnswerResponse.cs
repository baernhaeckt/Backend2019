namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionAnswerResponse
    {
        public bool IsCorrect { get; set; }

        public int AwardedPoints { get; set; }

        public string DetailedAnswer { get; set; }
    }
}