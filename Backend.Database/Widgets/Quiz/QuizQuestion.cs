using AspNetCore.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Database.Widgets.Quiz
{
    public class QuizQuestion : IMongoEntity
    {
        public string Question { get; set; }

        public IEnumerable<string> IncorrectAnswers { get; set; }

        public IEnumerable<string> CorrectAnswers { get; set; }

        public int Points { get; set; }

        public string DetailedAnswer { get; set; }
    }
}
