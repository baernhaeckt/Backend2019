using Silverback.Messaging.Messages;

namespace Backend.Core.Events
{
    public class QuizQuestionCorrectAnsweredEvent : IEvent
    {
        public QuizQuestionCorrectAnsweredEvent(int questionPoints) => QuestionPoints = questionPoints;

        public int QuestionPoints { get; }
    }
}