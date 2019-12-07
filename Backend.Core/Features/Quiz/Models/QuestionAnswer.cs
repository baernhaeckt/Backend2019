using System;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionAnswer
    {
        public Guid QuestionId { get; set; }

        public Guid AnswerId { get; set; }
    }
}