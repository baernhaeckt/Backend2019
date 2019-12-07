namespace Backend.Core.Features.Quiz.Commands
{
    public class AnswerQuizQuestionResult
    {
        public bool IsCorrect { get; set; }

        public int AwardedPoints { get; set; }

        public string? DetailedAnswer { get; set; }
    }
}