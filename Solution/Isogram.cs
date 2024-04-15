using System.Text.RegularExpressions;

namespace Exercism.Solution;

public static partial class Isogram
{
    public static bool IsIsogram(string word) => !RepeatingLetterPattern().IsMatch(word);

    [GeneratedRegex(@"(\p{L}).*\1", RegexOptions.IgnoreCase, "en")]
    private static partial Regex RepeatingLetterPattern();
}