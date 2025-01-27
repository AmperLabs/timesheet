namespace Timesheet.Common
{
    public static class StringExtensions
    {
        public static string NormalizeEmail(this string str)
        {
            var stringsToRemove = new List<string>
            {
                " ", ".", "-", "_"
            };

            foreach (var s in stringsToRemove)
            {
                str = str.Replace(s, "");
            }

            return str.ToLowerInvariant();
        }
    }
}
