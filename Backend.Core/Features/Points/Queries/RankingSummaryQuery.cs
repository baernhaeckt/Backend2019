using System;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Points.Queries
{
    public class RankingSummaryQuery : IQuery<RankingSummaryQueryResult>
    {
        public RankingSummaryQuery(Guid id) => Id = id;

        public Guid Id { get; }
    }
}
