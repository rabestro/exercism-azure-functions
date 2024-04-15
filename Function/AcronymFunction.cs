using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class AcronymFunction
{
    [Function("acronym")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("AcronymFunction");
        logger.LogInformation("Processing request...");

        string phrase;

        if (req.Method == HttpMethod.Get.ToString())
        {
            phrase = req.Query["phrase"];
        }
        else
        {
            var requestBody = await req.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<PhraseInput>(requestBody);
            phrase = data?.Phrase;
        }

        if (string.IsNullOrEmpty(phrase))
        {
            logger.LogWarning("No phrase provided in request.");
            return CreateJsonResponse(req, HttpStatusCode.BadRequest, new { error = "Please provide a phrase." });
        }

        var acronym = Acronym.Abbreviate(phrase);
        logger.LogInformation($"Acronym for phrase: {acronym}");

        return CreateJsonResponse(req, HttpStatusCode.OK, new { phrase, acronym });
    }

    private HttpResponseData CreateJsonResponse(HttpRequestData req, HttpStatusCode statusCode, object content)
    {
        var response = req.CreateResponse(statusCode);
        var result = JsonSerializer.Serialize(content);
        response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
        response.WriteString(result);
        return response;
    }

    private class PhraseInput
    {
        public string Phrase { get; set; }
    }
}