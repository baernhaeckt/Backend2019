using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;

namespace Backend.Database.Widgets.Quiz
{
    public class UserQuiz : IMongoEntity
    {
        public string UserId { get; set; }

        public Dictionary<DateTime, IList<UserQuizAnswer>> AnswersByDay { get; set; }
            = new Dictionary<DateTime, IList<UserQuizAnswer>>();
    }
}
