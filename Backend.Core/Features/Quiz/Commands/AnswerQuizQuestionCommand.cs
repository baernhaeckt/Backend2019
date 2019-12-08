using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Quiz.Commands
{
    public class AnswerQuizQuestionCommand : ICommand<AnswerQuizQuestionResult>
    {
        public AnswerQuizQuestionCommand(Guid userId, Guid questionId, Guid answerId)
        {
            UserId = userId;
            QuestionId = questionId;
            AnswerId = answerId;
        }

        public Guid UserId { get; }

        public Guid QuestionId { get; }

        public Guid AnswerId { get; }
    }
}