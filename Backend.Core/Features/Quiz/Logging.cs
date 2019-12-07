using System;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Quiz
{
    public static class Logging
    {

        public static void RetrieveQuizQuestionForToday(this ILogger logger, Guid userId)
        {
            logger.LogInformation(new EventId(1, typeof(Logging).Namespace), "Retrieve next quiz question which can be answered. UserId: {userId}.", userId);
        }

        public static void RetrieveQuizQuestionForTodaySuccessful(this ILogger logger, Guid userId, bool questionToAnswer)
        {
            logger.LogInformation(new EventId(2, typeof(Logging).Namespace), "Retrieved next quiz question which can be answered. Email: {userId}, QuestionToAnswer: {questionToAnswer}", userId, questionToAnswer);
        }

        public static void ExecuteAnswerQuizQuestion(this ILogger logger, Guid userId, Guid questionId)
        {
            logger.LogInformation(new EventId(3, typeof(Logging).Namespace), "Execute answer quiz question. UserId: {userId}, QuestionId: {questionId}", userId, questionId);
        }

        public static void ExecuteAnswerQuizQuestionSuccessful(this ILogger logger, Guid userId, Guid questionId)
        {
            logger.LogInformation(new EventId(4, typeof(Logging).Namespace), "Executed answer quiz question. UserId: {userId}, QuestionId: {questionId}", userId, questionId);
        }

    }
}