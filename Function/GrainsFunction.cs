using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class GrainsFunction
{
    [Function("grains")]
    public static async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("GrainsFunction");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var squareString = req.Query["square"];
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);

        if (string.IsNullOrEmpty(squareString))
        {
            await response.WriteStringAsync(JsonSerializer.Serialize(new { total = Grains.Total() }));
            return response;
        }

        if (!int.TryParse(squareString, out var square))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync("Invalid input: Please provide valid square number.");
            return errorResponse;
        }

        var result = JsonSerializer.Serialize(new { grains = Grains.Square(square) });
        await response.WriteStringAsync(result);
        return response;
    }
}