using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class IsogramFunction
{
    [Function("isogram")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("IsogramFunction");
        logger.LogInformation("Processing request...");

        string word;

        if (req.Method == HttpMethod.Get.ToString())
        {
            word = req.Query["word"];
        }
        else
        {
            var requestBody = await req.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<WordInput>(requestBody);
            word = data?.Word;
        }

        if (string.IsNullOrEmpty(word))
        {
            logger.LogWarning("No word provided in request.");
            return CreateJsonResponse(req, HttpStatusCode.BadRequest, new { error = "Please provide a word." });
        }

        var isIsogram = Isogram.IsIsogram(word);
        logger.LogInformation($"Word is isogram: {isIsogram}");

        return CreateJsonResponse(req, HttpStatusCode.OK, new { word, isIsogram });
    }

    private HttpResponseData CreateJsonResponse(HttpRequestData req, HttpStatusCode statusCode, object content)
    {
        var response = req.CreateResponse(statusCode);
        var result = JsonSerializer.Serialize(content);
        response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
        response.WriteString(result);
        return response;
    }

    private class WordInput
    {
        public string Word { get; set; }
    }
}