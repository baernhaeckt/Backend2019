using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Core.Framework;
using Backend.Infrastructure.Abstraction.Persistence;
using Microsoft.Extensions.Logging;

namespace Backend.Core.Features.Baseline.Queries
{
    internal class AllSufficientTypesQueryHandler : QueryHandler<IEnumerable<SufficientType>, AllSufficientTypesQuery>
    {
        public AllSufficientTypesQueryHandler(IReader reader, ILogger<AllSufficientTypesQueryHandler> logger)
            : base(reader, logger)
        {
        }

        public override async Task<IEnumerable<SufficientType>> ExecuteAsync(AllSufficientTypesQuery query) => await Reader.GetAllAsync<SufficientType>();
    }
}