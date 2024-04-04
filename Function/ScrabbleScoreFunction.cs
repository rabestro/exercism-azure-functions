using Exercism.Solution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Exercism.Function;

public class ScrabbleScoreFunction(ILogger<ScrabbleScoreFunction> logger)
{
    [Function("scrabble-score")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var word = req.Query["word"];

        if (string.IsNullOrEmpty(word) )
        {
            return new BadRequestObjectResult(new { message = "Please pass a 'word' on the query string" });
        }

        var score = ScrabbleScore.Score(word!);

        return new OkObjectResult(new { score });
    }
}