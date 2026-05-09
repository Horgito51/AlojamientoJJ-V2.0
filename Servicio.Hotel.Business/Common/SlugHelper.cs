using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Servicio.Hotel.Business.Common
{
    public static class SlugHelper
    {
        private static readonly Regex NonAllowedChars = new Regex(@"[^a-z0-9\\-\\s]", RegexOptions.Compiled);
        private static readonly Regex MultiSpace = new Regex(@"\\s+", RegexOptions.Compiled);
        private static readonly Regex MultiHyphen = new Regex(@"\\-+", RegexOptions.Compiled);

        public static string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var normalized = input.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            var noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);
            var cleaned = NonAllowedChars.Replace(noDiacritics, string.Empty);
            cleaned = MultiSpace.Replace(cleaned, " ").Trim();
            cleaned = cleaned.Replace(' ', '-');
            cleaned = MultiHyphen.Replace(cleaned, "-").Trim('-');
            return cleaned;
        }
    }
}

