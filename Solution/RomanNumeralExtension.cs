using System.Text;

namespace Exercism.Solution;

public static class RomanNumeralExtension
{
    private static readonly Dictionary<int, string> ArabicToRoman = new()
    {
        { 1000, "M" },
        { 900, "CM" },
        { 500, "D" },
        { 400, "CD" },
        { 100, "C" },
        { 90, "XC" },
        { 50, "L" },
        { 40, "XL" },
        { 10, "X" },
        { 9, "IX" },
        { 5, "V" },
        { 4, "IV" },
        { 1, "I" }
    };

    public static string ToRoman(this int value)
    {
        StringBuilder roman = new();
        foreach (var (threshold, numeral) in ArabicToRoman)
        {
            while (threshold <= value)
            {
                roman.Append(numeral);
                value -= threshold;
            }
        }

        return roman.ToString();
    }
}