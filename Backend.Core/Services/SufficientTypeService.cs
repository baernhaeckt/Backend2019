using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Database;

namespace Backend.Core.Services
{
    public class SufficientTypeService
    {
        private readonly IMongoOperation<SufficientType> _sufficientTypeRepository;

        public SufficientTypeService(IMongoOperation<SufficientType> sufficientTypeRepository)
        {
            _sufficientTypeRepository = sufficientTypeRepository;
        }

        public void GenerateSufficientTypes()
        {
            IList<SufficientType> sufficientTypes = new List<SufficientType>()
            {
                new SufficientType
                {
                    Title = "Energie",
                    Description = "Du hast Energie gespart.",
                    BaselinePoint = 250
                },
                new SufficientType
                {
                    Title = "Verpackung",
                    Description = "Du hast Verpackungslos eingekauft.",
                    BaselinePoint = 570
                },
                new SufficientType
                {
                    Title = "Food Waste",
                    Description = "Du hast Food Waste vermieden.",
                    BaselinePoint = 745
                },
                new SufficientType
                {
                    Title = "Wissen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt.",
                    BaselinePoint = 260
                },
                new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast deinen Besitz mit anderen geteilt.",
                    BaselinePoint = 155
                },
                new SufficientType
                {
                    Title = "Unterstützen",
                    Description = "Die hast gemeinnützige Dienstleistung geleistet.",
                    BaselinePoint = 450
                }
            };

            _sufficientTypeRepository.InsertMany(sufficientTypes);
        }

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes()
        {
            return await _sufficientTypeRepository.GetAllAsync();
        }
    }
}