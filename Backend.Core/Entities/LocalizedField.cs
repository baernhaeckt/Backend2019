using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Backend.Core.Entities
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "No collection semantic.")]
    public class LocalizedField : List<KeyValuePair<string, string>>
    {
        public static LocalizedField Empty { get; } = new LocalizedField();

        public string GetForCurrentCulture() => GetFor(CultureInfo.DefaultThreadCurrentUICulture);

        public string GetFor(CultureInfo? cultureInfo)
        {
            string key = cultureInfo?.TwoLetterISOLanguageName ?? Localization.DefaultCultureInfo.TwoLetterISOLanguageName;
            return this.SingleOrDefault(k => k.Key == key).Value ?? this.Single(k => k.Key == Localization.DefaultCultureInfo.TwoLetterISOLanguageName).Value;
        }
    }
}