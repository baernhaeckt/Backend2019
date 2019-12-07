using System;

namespace Backend.Core.Entities.Quiz
{
    public class Answer
    {
        public Guid Id { get; set; }

        public LocalizedField AnswerText { get; set; } = LocalizedField.Empty;

        public bool IsCorrect { get; set; }
    }
}