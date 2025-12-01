using System;
using DroneRace.Models;

namespace DroneRace.Services;

public class ApiRace
{
    private readonly ApiDataService _api;

    public ApiRace(ApiDataService api)
    {
        _api = api;
    }

    public async Task StartApiRace(RaceLog log)
    {
        log.Log("SYSTEM", "API Race started!");
        Console.WriteLine("Fetching route (number of nodes) and weather data...\n");

        ApiData? apiData;

        try
        {
            apiData = await _api.GetDataAsync();
        }
        catch (Exception ex)
        {
            log.Log("SYSTEM", $"API request failed: {ex.Message}");
            Console.WriteLine($"API Error: {ex.Message}");
            return;
        }

        if (apiData == null)
        {
            log.Log("SYSTEM", "API returned NULL data");
            throw new Exception("API returned no data");
        }

        var (checkpoints, weatherDelay) = WeatherMapper.Map(apiData);

        log.Log("SYSTEM", $"API mapping complete. nodes = {checkpoints}, WeatherDelay = {weatherDelay}ms");
        Console.WriteLine($"Album #{apiData.Id}: \"{apiData.Title}\"");
        Console.WriteLine($"Mapped nodes: {checkpoints}");
        Console.WriteLine($"Weather delay: additional {weatherDelay}ms\n");

        // Create drones with mapped weather - number of checkpoints and additional weather delay
        var drone1 = new DroneModel("Hawk", checkpoints, 300 + weatherDelay);
        var drone2 = new DroneModel("Eagle", checkpoints, 400 + weatherDelay);

        log.Log("SYSTEM", "Race starting...");
        Console.WriteLine("Race starting...\n");

        try
        {
            var task1 = NodeCounter(drone1, log);
            var task2 = NodeCounter(drone2, log);

            await Task.WhenAll(task1, task2);

            log.Log("SYSTEM", "API race completed successfully!");
            Console.WriteLine("\nRace completed!");
        }
        catch (Exception ex)
        {
            log.Log("SYSTEM", $"API Race failed: {ex.Message}");
            Console.WriteLine($"\n=== API RACE FAILURE ===\n{ex.Message}");
        }
    }

    private static async Task NodeCounter(DroneModel model, RaceLog log)
    {
        log.Log(model.Name, "Started");
        Console.WriteLine($"{model.Name} starting...");

        for (int node = 0; node <= model.MaxCheckpoints; node++)
        {
            log.Log(model.Name, $"Node {node}/{model.MaxCheckpoints}");
            Console.WriteLine($"{model.Name} reached node {node}/{model.MaxCheckpoints}");

            await Task.Delay(model.DelayMs);
        }

        log.Log(model.Name, "Finished");
        Console.WriteLine($"{model.Name} finished the race.");
    }
}
