using Exercism.Solution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Exercism.Function;

public class PigLatinFunction
{
    [Function("piglatin")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("PigLatinFunction");
        logger.LogInformation("Processing request...");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string sentence = data?.sentence;
        logger.LogInformation("Processing sentence:" + sentence);

        if (sentence == null) return new BadRequestObjectResult("Please pass a sentence in the request body json");

        var pigLatin = PigLatin.Translate(sentence);
        logger.LogInformation($"Translated sentence to Pig Latin: {pigLatin}");

        return new OkObjectResult(new { sentence, pigLatin });
    }
}