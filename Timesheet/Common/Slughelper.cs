using System.Text.RegularExpressions;

namespace Timesheet.Common
{
    public static class Slughelper
    {
        public static string GenerateSlug(string text)
        {
            var replacements = new Dictionary<string, string>
            {
                {" ", "-"},
                {"ä", "ae"},
                {"ö", "oe"},
                {"ü", "ue"},
                {"ß", "ss"}
            };

            // We want a lowercase Slug
            var slug = text.ToLowerInvariant();

            // Replace all replacements
            foreach (var replacement in replacements)
            {
                slug = slug.Replace(replacement.Key, replacement.Value);
            }

            // Remove everything that is not a letter, a digit or a dash
            slug = String.Concat(slug.Where(x => char.IsLetterOrDigit(x) || x == '-'));

            // Collapse multipe whitespaces
            slug = Regex.Replace(slug, "[ ]{2,}", " ");

            return slug.Trim();
        }
    }
}
