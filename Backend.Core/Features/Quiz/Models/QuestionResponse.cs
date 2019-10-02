using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }

        public string Question { get; set; } = string.Empty;

        public IList<string> Answers { get; set; } = Enumerable.Empty<string>().ToList();
    }
}