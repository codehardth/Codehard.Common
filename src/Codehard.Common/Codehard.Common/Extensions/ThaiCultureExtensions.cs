using System.Globalization;

namespace Codehard.Common.Extensions;

/// <summary>
/// Provides extension methods for Thai culture.
/// </summary>
public static class ThaiCultureExtensions
{
    
    /// <summary>
    /// Thai culture info.
    /// </summary>
    public static readonly CultureInfo ThaiCultureInfo = CultureInfo.GetCultureInfo("th-TH");
    
    /// <summary>
    /// This method converts a string to Thai number.
    /// </summary>
    /// <param name="src">The string to convert</param>
    /// <returns>The converted string</returns>
    public static string ToThaiNumber(this string src)
    {
        if (src.Length == 0)
        {
            return src;
        }

        // Up to 100kB
        const uint limit = 102_400;
        const int charSize = sizeof(char);
        const int startRange = 48;
        const int endRange = 57;

        return src.Length * charSize > limit
            ? ConvertToThaiNumberOnHeap(src)
            : ConvertToThaiNumberOnStack(src);

        static string ConvertToThaiNumberOnStack(string src)
        {
            Span<char> chars = stackalloc char[src.Length];

            for (var idx = 0; idx < src.Length; idx++)
            {
                chars[idx] = Convert(src[idx]);
            }

            return new string(chars);
        }

        static string ConvertToThaiNumberOnHeap(string src)
        {
            var chars = src.ToCharArray();

            for (var idx = 0; idx < src.Length; idx++)
            {
                chars[idx] = Convert(src[idx]);
            }

            return new string(chars);
        }

        static char Convert(char c)
        {
            var charCode = (int)c;

            return charCode is < startRange or > endRange ? c : (char)(charCode + 3616);
        }
    }

    /// <summary>
    /// This method converts a string to Thai date string.
    /// </summary>
    public static string ToThaiDateString(
        this DateTimeOffset dateTimeOffset,
        string format = "dd/MM/yyyy")
    {
        return dateTimeOffset.ToString(
            format,
            ThaiCultureInfo);
    }
}