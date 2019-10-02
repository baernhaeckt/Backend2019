using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class QuizQuestionCorrectAnsweredEvent : IEvent
    {
        public QuizQuestionCorrectAnsweredEvent(Guid userId, int questionPoints)
        {
            UserId = userId;
            QuestionPoints = questionPoints;
        }

        public Guid UserId { get; }

        public int QuestionPoints { get; }
    }
}