using System.Collections.Generic;
using System.Globalization;

namespace Backend.Core
{
    public static class Localization
    {
        private const string DefaultCulture = "de";

        public static readonly CultureInfo DefaultCultureInfo = new CultureInfo(DefaultCulture);

        public static IList<CultureInfo> SupportedCultures { get; } = new[]
        {
            DefaultCultureInfo

            // new CultureInfo("en"),
            // new CultureInfo("fr"),
            // new CultureInfo("it")
        };
    }
}