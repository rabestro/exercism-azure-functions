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

        var result = planet switch
        {
            "Earth" => JsonSerializer.Serialize(new { age = spaceAge.OnEarth() }),
            "Mercury" => JsonSerializer.Serialize(new { age = spaceAge.OnMercury() }),
            "Venus" => JsonSerializer.Serialize(new { age = spaceAge.OnVenus() }),
            "Mars" => JsonSerializer.Serialize(new { age = spaceAge.OnMars() }),
            "Jupiter" => JsonSerializer.Serialize(new { age = spaceAge.OnJupiter() }),
            "Saturn" => JsonSerializer.Serialize(new { age = spaceAge.OnSaturn() }),
            "Uranus" => JsonSerializer.Serialize(new { age = spaceAge.OnUranus() }),
            "Neptune" => JsonSerializer.Serialize(new { age = spaceAge.OnNeptune() }),
            "" or null => JsonSerializer.Serialize(new
            {
                earth = spaceAge.OnEarth(),
                mercury = spaceAge.OnMercury(),
                venus = spaceAge.OnVenus(),
                mars = spaceAge.OnMars(),
                jupiter = spaceAge.OnJupiter(),
                saturn = spaceAge.OnSaturn(),
                uranus = spaceAge.OnUranus(),
                neptune = spaceAge.OnNeptune()
            }),
            _ => "not a planet"
        };

        response.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await response.WriteStringAsync(result);
        return response;
    }
}