using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.MongoDB;
using Backend.Core.Security.Extensions;
using Backend.Database;

namespace Backend.Core.Services
{
    public class SufficientTypeService : PersonalizedService
    {
        private readonly IMongoOperation<SufficientType> _sufficientTypeRepository;

        public SufficientTypeService(IMongoOperation<SufficientType> sufficientTypeRepository, IMongoOperation<User> userRepository, ClaimsPrincipal principal)
            : base(userRepository, principal)
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
                    BaselinePoint = 250,
                    BaselineCo2Saving = 5.89
                },
                new SufficientType
                {
                    Title = "Verpackung",
                    Description = "Du hast Verpackungslos eingekauft.",
                    BaselinePoint = 570,
                    BaselineCo2Saving = 9.67
                },
                new SufficientType
                {
                    Title = "Food Waste",
                    Description = "Du hast Food Waste vermieden.",
                    BaselinePoint = 745,
                    BaselineCo2Saving = 10.89
                },
                new SufficientType
                {
                    Title = "Wissen",
                    Description = "Du hast dein Suffizienz mit anderen geteilt.",
                    BaselinePoint = 260,
                    BaselineCo2Saving = 12.88
                },
                new SufficientType
                {
                    Title = "Teilen",
                    Description = "Du hast deinen Besitz mit anderen geteilt.",
                    BaselinePoint = 155,
                    BaselineCo2Saving = 4.99
                },
                new SufficientType
                {
                    Title = "Unterstützen",
                    Description = "Die hast gemeinnützige Dienstleistung geleistet.",
                    BaselinePoint = 450,
                    BaselineCo2Saving = 5.66
                }
            };

            _sufficientTypeRepository.InsertMany(sufficientTypes);
        }

        public async Task<IEnumerable<SufficientType>> GetSufficientTypes()
        {
            return await _sufficientTypeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<UserSufficientType>> GetSufficientTypesFromUser()
        {
            User user = await UserRepository.GetByIdAsync(Principal.Id());
            return user.PointActions.GroupBy(p => p.SufficientType.Title).Select(p => new UserSufficientType
            {
                Title = p.Key,
                Point = p.Sum(o => o.Point),
                Co2Saving = p.Sum(c => c.Co2Saving)
            });
        }
    }
}