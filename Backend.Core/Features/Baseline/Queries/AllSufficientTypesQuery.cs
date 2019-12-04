using System.Collections.Generic;
using Backend.Core.Entities;
using Silverback.Messaging.Messages;

namespace Backend.Core.Features.Baseline.Queries
{
    public class AllSufficientTypesQuery : IQuery<IEnumerable<SufficientType>>
    {
    }
}