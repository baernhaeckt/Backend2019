using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionAnswer
    {
        public Guid QuestionId { get; set; }

        public IEnumerable<string> Answers { get; set; } = Enumerable.Empty<string>();
    }
}