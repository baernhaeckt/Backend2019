using System;
using System.Collections.Generic;

namespace Backend.Database.Entities.Widgets.Quiz
{
    public class UserQuiz : Entity
    {
        public Guid UserId { get; set; }

        public IDictionary<string, IList<UserQuizAnswer>> AnswersByDay { get; set; }
            = new Dictionary<string, IList<UserQuizAnswer>>();
    }
}