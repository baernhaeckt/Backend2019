using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Quiz.Queries
{
    public class QuizQuestionForTodayQueryResult
    {
        public QuizQuestionForTodayQueryResult(Guid id, string question, IList<string> answers)
        {
            Id = id;
            Question = question;
            Answers = answers;
        }

        public Guid Id { get; }

        public string Question { get; }

        public IList<string> Answers { get; }
    }
}