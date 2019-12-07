using System;
using Backend.Core.Features.Quiz.Models;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Quiz.Commands
{
    public class AnswerQuizQuestionCommand : ICommand<QuestionAnswerResponse>
    {
        public AnswerQuizQuestionCommand(Guid userId, QuestionAnswer questionAnswer)
        {
            UserId = userId;
            QuestionAnswer = questionAnswer;
        }

        public Guid UserId { get; }

        public QuestionAnswer QuestionAnswer { get; }
    }
}