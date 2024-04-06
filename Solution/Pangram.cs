namespace Exercism.Solution;

public static class Pangram
{
    private const int LatinAlphabetLetterCount = 'Z' - 'A' + 1;

    public static bool IsPangram(string input) => input.ToUpper()
        .Where(IsLatinAlphabetLetter)
        .Distinct()
        .Take(LatinAlphabetLetterCount)
        .Count() == LatinAlphabetLetterCount; 

    private static bool IsLatinAlphabetLetter(char c) => c is >= 'A' and <= 'Z';
}