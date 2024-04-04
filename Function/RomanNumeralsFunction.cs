using Exercism.Solution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

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
        
        return new OkObjectResult(new { roman = number.ToRoman()});
    }
}
