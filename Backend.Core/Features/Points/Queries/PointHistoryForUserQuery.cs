using System;
using System.Collections.Generic;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Points.Queries
{
    public class PointHistoryForUserQuery : IQuery<IEnumerable<PointHistoryForUserQueryResult>>
    {
        public PointHistoryForUserQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}