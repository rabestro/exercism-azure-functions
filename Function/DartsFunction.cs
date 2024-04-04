using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

public class DartsFunction
{
    [Function("darts-score")]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("DartsFunction");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var xString = req.Query["x"];
        var yString = req.Query["y"];

        if (!double.TryParse(xString, out var x) || !double.TryParse(yString, out var y))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync("Invalid input: Please provide valid x and y coordinates. Both x and y must be numbers.");
            return errorResponse;
        }

        var score = Darts.Score(x, y);
        var response = req.CreateResponse(HttpStatusCode.OK);
        var result = JsonSerializer.Serialize(new { score });

        response.Headers.Add("Content-Type", MediaTypeNames.Application.Json);
        await response.WriteStringAsync(result);

        return response;
    }
}