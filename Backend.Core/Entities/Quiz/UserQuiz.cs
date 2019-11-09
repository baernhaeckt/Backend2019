﻿using System;
using System.Collections.Generic;
using Backend.Infrastructure.Abstraction.Persistence;

namespace Backend.Core.Entities.Quiz
{
    public class UserQuiz : Entity
    {
        public Guid UserId { get; set; }

        public IDictionary<string, IList<UserQuizAnswer>> AnswersByDay { get; set; }
            = new Dictionary<string, IList<UserQuizAnswer>>();
    }
}