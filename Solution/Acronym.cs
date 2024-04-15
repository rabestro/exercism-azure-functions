using System.Text.RegularExpressions;

namespace Exercism.Solution;

public static partial class Acronym
{
    public static string Abbreviate(string phrase) =>
        WordsPattern().Replace(phrase, "${FirstLetter}").ToUpper();

    [GeneratedRegex(@"\P{L}*(?<FirstLetter>\p{L})[\p{L}']*\P{L}*")]
    private static partial Regex WordsPattern();
}