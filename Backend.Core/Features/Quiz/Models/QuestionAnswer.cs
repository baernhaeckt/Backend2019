using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionAnswer
    {
        public Guid QuestionId { get; set; }

        public IEnumerable<string> Answers { get; set; }
    }
}