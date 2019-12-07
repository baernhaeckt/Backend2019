using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Entities.Quiz
{
    public class Question : Entity
    {
        public LocalizedField QuestionText { get; set; } = LocalizedField.Empty;

        public LocalizedField ExplanationText { get; set; } = LocalizedField.Empty;

        public IList<Answered> AnsweredBy { get; set; } = Enumerable.Empty<Answered>().ToList();

        public IList<Answer> Answers { get; set; } = Enumerable.Empty<Answer>().ToList();

        public int Points { get; set; }
    }
}