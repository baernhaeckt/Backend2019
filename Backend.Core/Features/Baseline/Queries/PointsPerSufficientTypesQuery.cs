using System;
using System.Collections.Generic;
using Backend.Core.Entities;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Baseline.Queries
{
    public class PointsPerSufficientTypesQuery : IQuery<IEnumerable<UserSufficientType>>
    {
        public PointsPerSufficientTypesQuery(Guid userId) => UserId = userId;

        public Guid UserId { get; }
    }
}