using System;
using System.Collections.Generic;

namespace Backend.Core.Features.Newsfeed.Events
{
    public class NewsfeedEvent
    {
        public string Title { get; protected set; } = string.Empty;

        public string Message { get; protected set; } = string.Empty;

        // success oder info
        public string Variant { get; protected set; } = string.Empty;

        public IList<Guid> Audience { get; } = new List<Guid>();
    }
}