using System;
using System.Collections.Generic;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Baseline.Queries
{
    public class PointsPerSufficientTypesQuery : IQuery<IEnumerable<SufficientTypesQueryResult>>
    {
        public PointsPerSufficientTypesQuery(Guid userId) => UserId = userId;

        public Guid UserId { get; }
    }
}