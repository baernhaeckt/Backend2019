using System;
using System.Collections.Generic;
using System.Text;
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
                    Description = "Du hast Energie gespart."
                },
                new SufficientType
                {
                    Title = "Verpackung",
                    Description = "Du hast Verpackungslos eingekauft."
                },
                new SufficientType
                {
                    Title = "Food Waste",
                    Description = "Du hast Food Waste vermieden."
                },
                new SufficientType
                {
                    Title = "Wissen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt."
                },
                new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast deinen Besitz mit anderen geteilt."
                },
                new SufficientType
                {
                    Title = "Unterstützen",
                    Description = "Die hast gemeinnützige Dienstleistung geleistet."
                }
            };

            _sufficientTypeRepository.InsertMany(sufficientTypes);
        }
    }
}
