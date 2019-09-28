using System;
using System.Collections.Generic;

namespace Backend.Models.Widgets.Quiz
{
    public class QuestionAnswer
    {
        public Guid QuestionId { get; set; }

        public IEnumerable<string> Answers { get; set; }
    }
}