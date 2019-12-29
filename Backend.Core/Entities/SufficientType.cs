using System.Text.Json.Serialization;

namespace Backend.Core.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SufficientType
    {
        Empty = 0,

        Knowledge = 1,

        Energy = 2,

        Packing = 3,

        FoodWaste = 4,

        Share = 5,

        Support = 6
    }
}