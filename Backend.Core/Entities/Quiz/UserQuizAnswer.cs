using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Entities.Quiz
{
    public class UserQuizAnswer
    {
        public Guid QuizQuestionId { get; set; }

        public IList<string> SelectedAnswer { get; set; } = Enumerable.Empty<string>().ToList();

        public bool IsCorrect { get; set; }

        public int Points { get; set; }
    }
}