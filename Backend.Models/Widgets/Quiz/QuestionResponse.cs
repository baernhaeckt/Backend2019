using System;
using System.Collections.Generic;

namespace Backend.Models.Widgets.Quiz
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }

        public string Question { get; set; }

        public IEnumerable<string> Answers { get; set; }
    }
}