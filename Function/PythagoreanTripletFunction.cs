using Exercism.Solution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

public class PythagoreanTripletFunction
{
    [Function("pythagorean-triplet")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pythagorean-triplet/{sum:int?}")]
        HttpRequestData req,
        int sum,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("PythagoreanTripletFunction");
        logger.LogInformation("Processing request...");

        if (sum <= 0)
        {
            logger.LogWarning("Invalid sum provided in request.");
            return new BadRequestObjectResult(new { error = "Please provide a valid sum." });
        }

        var triplets = PythagoreanTriplet.TripletsWithSum(sum)
            .Select(t => new Triplet(t.a, t.b, t.c))
            .ToList();
        logger.LogInformation($"Found {triplets.Count} triplets with sum {sum}");

        return new OkObjectResult(new { sum, triplets });
    }
}

public record Triplet(int A, int B, int C);