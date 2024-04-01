using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Exercism.Solution;

public class ArmstrongNumbersFunction(ILogger<ArmstrongNumbersFunction> logger)
{
    [Function("armstrong-numbers")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var numberString = req.Query["number"];

        if (string.IsNullOrEmpty(numberString) || !long.TryParse(numberString, out var number))
        {
            return new BadRequestObjectResult(new { message = "Please pass a number on the query string" });
        }

        var isArmstrongNumber = IsArmstrongNumber(number);

        return new OkObjectResult(new { isArmstrongNumber });
    }

    private static bool IsArmstrongNumber(long number)
    {
        var exponent = number.ToString().Length;
        var sumOfDigits = number.ToString()
            .ToCharArray()
            .Sum(digit => (long)Math.Pow(int.Parse(digit.ToString()), exponent));

        return sumOfDigits == number;
    }
}