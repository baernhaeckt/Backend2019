using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }

        public string Question { get; set; }

        public IEnumerable<string> Answers { get; set; }
    }
}