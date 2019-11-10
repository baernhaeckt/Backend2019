using System.Collections.Generic;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Points.Queries
{
    public class RankingByZipQuery : IQuery<IEnumerable<RankingQueryResult>>
    {
        public RankingByZipQuery(string zip) => Zip = zip;

        public string Zip { get; }
    }
}