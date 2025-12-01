using System;
using DroneRace.Models;

namespace DroneRace.Services;

public class AsyncRace
{
    private static readonly Random _rnd = new Random();

    public static async Task StartAsyncRace(RaceLog log)
    {
        Console.WriteLine("Race with Async tasks!\n\n");
        log.Log("SYSTEM", "AsyncRace started!");

        var drone1 = new DroneModel("Hawk", 5, 300);
        var drone2 = new DroneModel("Eagle", 5, 400);

        // Start async tasks for each drone
        var task1 = NodeCounter(drone1, log);
        var task2 = NodeCounter(drone2, log);

        try
        {
            await Task.WhenAll(task1, task2);
            log.Log("SYSTEM", "All drones finished successfully!");
            Console.WriteLine("Race completed!");
        }
        catch (Exception ex)
        {
            log.Log("SYSTEM", $"Race failed: {ex.InnerException?.Message ?? ex.Message}");
            Console.WriteLine("\n=== ASYNC RACE FAILURE ===");
            Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
        }
    }

    private static async Task NodeCounter(DroneModel model, RaceLog log)
    {
        log.Log(model.Name, "Started");
        Console.WriteLine($"{model.Name} starting...");

        for (int node = 0; node <= model.MaxCheckpoints; node++)
        {
            // Random extra delay to simulate variability
            int randomExtraDelay = _rnd.Next(-401, 500);
            int totalDelay = model.DelayMs + randomExtraDelay;

            log.Log(model.Name, $"Reached node {node}/{model.MaxCheckpoints}. Base delay: {model.DelayMs}ms | Random: {randomExtraDelay}ms");
            Console.WriteLine($"{model.Name} has reached node {node}/{model.MaxCheckpoints}");

            // Simulated failure: Negative delay = fuel miscalculation
            if (totalDelay < 0)
            {
                log.Log(model.Name, $"FUEL MISCALCULATION! Total delay = {totalDelay}ms at node {node}");
                throw new InvalidOperationException($"{model.Name} suffered a FUEL MISCALCULATION at node {node}! (Total delay = {totalDelay}ms)");
            }

            log.Log(model.Name, $"Delaying for {totalDelay}ms...");
            await Task.Delay(totalDelay);

            // Simulated failure: 2% chance of engine failure
            if (_rnd.NextDouble() < 0.02)
            {
                log.Log(model.Name, $"ENGINE FAILURE at node {node}!");
                throw new InvalidOperationException($"{model.Name} ENGINE FAILURE at node {node}!");
            }

            log.Log(model.Name, $"Resumed after {totalDelay}ms delay at node {node}");
        }

        log.Log(model.Name, "Finished");
        Console.WriteLine($"{model.Name} has completed the race...");
    }
}

/* Del C – “Async orchestration” (async/await)

Gjør samme scenario mer lesbart med async/await.

    Implementer droneflyvning som en async-metode med await Task.Delay for hvert steg.
    Orkestrer flere droner med await Task.WhenAll.
    Try/catch rundt orchestrering for enkel feilrapportering.
    Sammenlign kort med Del B – hvor mye boilerplate forsvinner, og hva betyr det for vedlikehold?
*/