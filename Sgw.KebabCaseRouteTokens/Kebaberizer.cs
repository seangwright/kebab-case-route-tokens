using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Sgw.KebabCaseRouteTokens
{
    /// <summary>
    /// Performs string convertions to kebab-case
    /// </summary>
    public static class Kebaberizer
    {
        /// <summary>
        /// Returns the lower-kebab-case version string of the provided PascalCase string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string PascalToLowerKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}
