using System;
using Backend.Core.Framework.Web;

namespace Backend.Core.Features.Quiz.Models
{
    public class QuestionAnswerRequest
    {
        [NotEmpty]
        public Guid QuestionId { get; set; }

        [NotEmpty]
        public Guid AnswerId { get; set; }
    }
}