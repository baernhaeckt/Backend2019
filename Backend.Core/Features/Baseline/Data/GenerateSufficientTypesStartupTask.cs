using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Core.Entities;
using Backend.Infrastructure.Hosting.Abstraction;
using Backend.Infrastructure.Persistence.Abstraction;

namespace Backend.Core.Features.Baseline.Data
{
    public class GenerateSufficientTypesStartupTask : IStartupTask
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateSufficientTypesStartupTask(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (await _unitOfWork.CountAsync<SufficientType>() != 0)
            {
                return;
            }

            IList<SufficientType> sufficientTypes = new List<SufficientType>
            {
                new SufficientType
                {
                    Title = "Energie",
                    Description = "Du hast Energie gespart.",
                    BaselinePoint = 100,
                    BaselineCo2Saving = 5.89
                },
                new SufficientType
                {
                    Title = "Verpackung",
                    Description = "Du hast Verpackungslos eingekauft.",
                    BaselinePoint = 70,
                    BaselineCo2Saving = 7.67
                },
                new SufficientType
                {
                    Title = "Food Waste",
                    Description = "Du hast Food Waste vermieden.",
                    BaselinePoint = 90,
                    BaselineCo2Saving = 4.89
                },
                new SufficientType
                {
                    Title = "Wissen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt.",
                    BaselinePoint = 50,
                    BaselineCo2Saving = 1.88
                },
                new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast deinen Besitz mit anderen geteilt.",
                    BaselinePoint = 65,
                    BaselineCo2Saving = 3.99
                },
                new SufficientType
                {
                    Title = "Unterstützen",
                    Description = "Die hast gemeinnützige Dienstleistung geleistet.",
                    BaselinePoint = 35,
                    BaselineCo2Saving = 2.66
                }
            };

            await _unitOfWork.InsertManyAsync(sufficientTypes);
        }
    }
}