using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

public class ArmstrongNumbersFunction(ILogger<ArmstrongNumbersFunction> logger)
{
    [Function("armstrong-numbers")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "armstrong-numbers/{number:long?}")]
        HttpRequest req, long? number)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        if (number == null)
            return new BadRequestObjectResult(new { message = "Please pass a number on the query string" });

        return new OkObjectResult(new { isArmstrongNumber = IsArmstrongNumber(number.Value) });
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