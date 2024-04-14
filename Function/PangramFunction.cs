using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Exercism.Solution;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Exercism.Function;

public class PangramFunction
{
    [Function("pangram")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("PangramFunction");
        logger.LogInformation("Processing 'pangram' request...");

        var requestBody = await req.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<SentenceInput>(requestBody);
        var sentence = data?.Sentence;
        
        var response = req.CreateResponse();
        response.Headers.Add(HeaderNames.ContentType, MediaTypeNames.Application.Json);
        object content;
        
        if (string.IsNullOrEmpty(sentence))
        {
            logger.LogWarning("No sentence provided in request.");
            content = new { error = "Please provide a sentence." };
            response.StatusCode = HttpStatusCode.BadRequest;
        }
        else
        {
            var isPangram = Pangram.IsPangram(sentence);
            logger.LogInformation($"Sentence is pangram: {isPangram}");
            content = new { sentence, isPangram };
            response.StatusCode = HttpStatusCode.OK;
        }
        var result = JsonSerializer.Serialize(content);
        await response.WriteStringAsync(result);
        return response;
    }

    private class SentenceInput
    {
        public string? Sentence { get; init; }
    }
}