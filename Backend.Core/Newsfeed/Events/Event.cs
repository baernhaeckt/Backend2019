using System;
using System.Collections.Generic;

namespace Backend.Core.Newsfeed
{
    public class Event
    {
        public string title { get; protected set; }

        public string message { get; protected set; }

        // success oder info
        public string variant { get; protected set; }

        public IList<Guid> Audience { get; } = new List<Guid>();
    }
}