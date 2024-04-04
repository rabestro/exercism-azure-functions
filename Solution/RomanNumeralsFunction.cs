using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Exercism.Solution;

public class RomanNumeralsFunction(ILogger<RomanNumeralsFunction> logger)
{
    [Function("roman-numerals")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        logger.LogInformation("Roman Numerals function processed a request.");

        var numberString = req.Query["number"];

        if (string.IsNullOrEmpty(numberString) || !int.TryParse(numberString, out var number))
        {
            return new BadRequestObjectResult(new { message = "Please pass a number on the query string" });
        }

        var roman = number.ToRoman();
        return new OkObjectResult(new { roman });
    }
}

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