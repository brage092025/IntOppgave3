using DroneRace.Models;
using DroneRace.Interfaces;

namespace DroneRace.Services;

public class TaskRace
{
    private static readonly Random _rnd = new Random(); // single Random instance for proper randomness

    public static Task StartTaskRace(IRaceLog log)
    {
        log.Log("SYSTEM", "Dronerace with Tasks and TaskCompletionSource started!");
        Console.WriteLine("Dronerace with Tasks and TaskCompletionSource!\n\n");

        var masterTsc = new TaskCompletionSource<bool>();

        var drone1 = new DroneModel("Hawk", 5, 300);
        var drone2 = new DroneModel("Eagle", 5, 400);

        var task1 = TscCounter(drone1, log);
        var task2 = TscCounter(drone2, log);

        Task.WhenAll(task1, task2).ContinueWith(all =>
        {
            if (all.IsFaulted)
            {
                log.Log("SYSTEM", "One or more drones failed.");
                masterTsc.SetException(all.Exception!);
            }
            else
            {
                log.Log("SYSTEM", "All drones finished!");
                masterTsc.SetResult(true);
            }
        });

        return masterTsc.Task;
    }

    private static Task TscCounter(DroneModel drone, IRaceLog log)
    {
        var tsc = new TaskCompletionSource();

        Task.Run(() =>
        {
            try
            {
                log.Log(drone.Name, "Starting the race!");
                CountWithTaskAwaiter(drone, tsc, log);
            }
            catch (Exception ex)
            {
                log.Log(drone.Name, $"Exception thrown BEFORE loop: {ex.Message}");
                tsc.SetException(ex);
            }
        });

        return tsc.Task;
    }

    private static void CountWithTaskAwaiter(DroneModel drone, TaskCompletionSource tsc, IRaceLog log)
    {
        int currentNode = 0;

        void CountContinuation()
        {
            try
            {
                if (currentNode <= drone.MaxCheckpoints)
                {
                    int randomExtraDelay = _rnd.Next(-401, 500); // allows negative delay
                    int totalDelay = drone.DelayMs + randomExtraDelay;

                    log.Log(drone.Name, $"Reached node {currentNode}/{drone.MaxCheckpoints}. Base delay: {drone.DelayMs}ms | Random: {randomExtraDelay}ms");
                    Console.WriteLine($"{drone.Name} has reached node {currentNode} of {drone.MaxCheckpoints}");

                    // --- Simulated Failure: Negative delay = FUEL Error ---
                    if (totalDelay < 0)
                    {
                        log.Log(drone.Name,
                            $"FUEL MISCALCULATION! Total delay < 0 ({totalDelay}ms) at node {currentNode}");

                        tsc.SetException(new InvalidOperationException(
                            $"{drone.Name} suffered a FUEL MISCALCULATION at node {currentNode}! (Total delay = {totalDelay}ms)"));
                        return;
                    }

                    log.Log(drone.Name, $"Delaying for {totalDelay}ms...");

                    var delayTask = Task.Delay(totalDelay);
                    var awaiter = delayTask.GetAwaiter();

                    awaiter.OnCompleted(() =>
                    {
                        // --- Simulated Failure: Engine failure ---
                        if (_rnd.NextDouble() < 0.02) // 2% chance of engine failure
                        {
                            log.Log(drone.Name, $"ENGINE FAILURE at node {currentNode}!");

                            tsc.SetException(new InvalidOperationException($"{drone.Name} ENGINE FAILURE at node {currentNode}!"));
                            return;
                        }

                        log.Log(drone.Name, $"Resuming after {totalDelay}ms delay at node {currentNode}");

                        currentNode++;
                        CountContinuation();
                    });
                }
                else
                {
                    log.Log(drone.Name, "Completed all checkpoints!");
                    tsc.SetResult();
                }
            }
            catch (Exception ex)
            {
                log.Log(drone.Name, $"Unexpected error inside loop: {ex.Message}");
                tsc.SetException(ex);
            }
        }

        CountContinuation();
    }
}



/* Del B – “Tower says go!” (Task + TaskCompletionSource)

Koordinere asynkron prosess og signalere ferdig/feil eksplisitt.

    Implementer droneflyvning med Task og TaskCompletionSource (en TCS per drone som signaliserer når ruten er ferdig).
    Start minst to droner og bruk Task.WhenAll for å fortsette når alle er ferdige.
    Simuler minst ett feilscenario (for eksempel negativ DelayMs eller et kunstig “motorfeil” på et bestemt checkpoint) og bekreft at feil propageres via TCS/Task.Exception.
    Skriv i refleksjon.md hvordan denne modellen skiller seg fra Thread/Join mht. kontroll og kompleksitet. */
