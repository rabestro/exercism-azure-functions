using Exercism.Solution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

public class ChangeFunction(ILogger<ChangeFunction> logger)
{
    [Function("change")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        [Microsoft.Azure.Functions.Worker.Http.FromBody]
        ChangeInput data)
    {
        logger.LogInformation("ChangeFunction processed a request.");

        try
        {
            var fewestCoins = Change.FindFewestCoins(data.Coins, data.Target);
            return new OkObjectResult(new { fewestCoins });
        }
        catch (ArgumentException ex)
        {
            return new BadRequestObjectResult(new { error = ex.Message });
        }
    }

    public record ChangeInput(int[] Coins, int Target);
}