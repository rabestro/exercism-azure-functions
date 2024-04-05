using System.Net;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class SqueakyCleanFunction
{
    [Function("squeaky-clean")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("SqueakyCleanFunction");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        string? identifier;

        if (req.Method == HttpMethod.Get.ToString())
        {
            identifier = req.Query["identifier"];
        }
        else
        {
            var requestBody = await req.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IdentifierInput>(requestBody);
            identifier = data?.Identifier;
        }

        if (string.IsNullOrEmpty(identifier))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync("Please provide an identifier.");
            return errorResponse;
        }

        var cleanedIdentifier = Identifier.Clean(identifier);
        var response = req.CreateResponse(HttpStatusCode.OK);

        var result = JsonSerializer.Serialize(new { cleanedIdentifier });

        response.Headers.Add(HeaderNames.ContentType, "application/json; charset=utf-8");
        await response.WriteStringAsync(result);

        return response;
    }

    private class IdentifierInput
    {
        public string Identifier { get; set; }
    }
}