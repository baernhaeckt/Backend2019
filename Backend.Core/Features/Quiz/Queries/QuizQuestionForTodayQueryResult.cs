using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Quiz.Queries
{
    public class QuizQuestionForTodayQueryResult
    {
        public QuizQuestionForTodayQueryResult(Guid id, string question, IEnumerable<KeyValuePair<Guid, string>> answers)
        {
            Id = id;
            Question = question;
            Answers = answers;
        }

        public Guid Id { get; }

        public string Question { get; }

        public IEnumerable<KeyValuePair<Guid, string>> Answers { get; }
    }
}