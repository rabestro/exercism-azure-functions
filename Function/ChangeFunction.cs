using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class ChangeFunction
{
    [Function("change")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("ChangeFunction");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var requestBody = await req.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ChangeInput>(requestBody);

        if (data?.Coins == null || data.Target == null)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync("Please provide coins and target.");
            return errorResponse;
        }

        try
        {
            var fewestCoins = Change.FindFewestCoins(data.Coins, data.Target.Value);
            var response = req.CreateResponse(HttpStatusCode.OK);

            var result = JsonSerializer.Serialize(new { fewestCoins });

            response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
            await response.WriteStringAsync(result);

            return response;
        }
        catch (ArgumentException ex)
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync(ex.Message);
            return errorResponse;
        }
    }

    private class ChangeInput
    {
        public int[] Coins { get; set; }
        public int? Target { get; set; }
    }
}