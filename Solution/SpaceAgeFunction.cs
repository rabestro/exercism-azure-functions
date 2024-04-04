using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Exercism.Solution;

public class SpaceAgeFunction(ILogger<SpaceAgeFunction> logger)
{
    [Function("space-age")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
        HttpRequestData req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");

        var secondsString = req.Query["seconds"];
        var planet = req.Query["planet"];

        if (!int.TryParse(secondsString, out var seconds))
        {
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResponse.WriteStringAsync("Please provide a valid number of seconds.");
            return errorResponse;
        }

        logger.LogInformation($"Seconds: {seconds}, Planet: {planet}");
        var spaceAge = new SpaceAge(seconds);
        var response = req.CreateResponse(HttpStatusCode.OK);

        object result = planet switch
        {
            "Earth" => new { age = spaceAge.OnEarth() },
            "Mercury" => new { age = spaceAge.OnMercury() },
            "Venus" => new { age = spaceAge.OnVenus() },
            "Mars" => new { age = spaceAge.OnMars() },
            "Jupiter" => new { age = spaceAge.OnJupiter() },
            "Saturn" => new { age = spaceAge.OnSaturn() },
            "Uranus" => new { age = spaceAge.OnUranus() },
            "Neptune" => new { age = spaceAge.OnNeptune() },
            "" or null => new
            {
                earth = spaceAge.OnEarth(),
                mercury = spaceAge.OnMercury(),
                venus = spaceAge.OnVenus(),
                mars = spaceAge.OnMars(),
                jupiter = spaceAge.OnJupiter(),
                saturn = spaceAge.OnSaturn(),
                uranus = spaceAge.OnUranus(),
                neptune = spaceAge.OnNeptune()
            },
            _ => new { error = "not a planet" }
        };

        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(JsonSerializer.Serialize(result));
        return response;
    }
}