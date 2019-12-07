using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Quiz.Queries
{
    public class QuizQuestionForTodayQuery : IQuery<QuizQuestionForTodayQueryResult>
    {
        public QuizQuestionForTodayQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}