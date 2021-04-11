using System.Globalization;

namespace StartupOrganizer
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string s) => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
    }
}
