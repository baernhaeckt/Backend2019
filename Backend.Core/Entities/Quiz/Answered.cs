using System;

namespace Backend.Core.Entities.Quiz
{
    public class Answered
    {
        public Guid UserId { get; set; }

        public DateTimeOffset AnsweredAt { get; set; }

        public Guid AnswerId { get; set; }
    }
}