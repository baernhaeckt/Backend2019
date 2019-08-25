using Backend.Models.Widgets.Quiz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Core.Services.Widgets
{
    public interface IQuizService
    {
        Task<QuestionResponse> Get();

        Task<QuestionAnswerResponse> Answer(QuestionAnswer answer);

        IEnumerable<SubmittedQuestionAnswer> SubmittedAnswersForToday { get; }

        IEnumerable<SubmittedQuestionAnswer> GetSubmittedAnswersForDay(DateTime day);
    }
}
