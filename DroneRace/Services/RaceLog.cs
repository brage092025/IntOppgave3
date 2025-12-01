using System.Collections.Concurrent;
using System.Diagnostics;
using DroneRace.Interfaces;
namespace DroneRace.Services;

public class RaceLog : IRaceLog
{
    private readonly ConcurrentBag<LogEntry> _entries = new();
    private readonly ConcurrentDictionary<string, long> _lastDroneTimes = new();
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    public void Log(string droneName, string message)
    {
        long now = _stopwatch.ElapsedMilliseconds;
        long last = _lastDroneTimes.GetOrAdd(droneName, now);
        long delta = now - last; // time since last event for this drone

        _entries.Add(new LogEntry
        {
            Timestamp = DateTime.Now,
            ElapsedMs = now,
            DeltaMs = delta,
            Drone = droneName,
            Message = message
        });

        _lastDroneTimes[droneName] = now; // update last-event time
    }

    public IReadOnlyList<LogEntry> GetLogs() => _entries.ToArray();


    public void PrintChronological()
    {
        Console.WriteLine("\n\n===== RACE LOG OUTPUT (CHRONOLOGICAL) =====\n");

        foreach (var log in _entries.OrderBy(l => l.ElapsedMs))
        {
            Console.WriteLine(
                $"{log.Timestamp:HH:mm:ss.fff} " +
                $"(+{log.ElapsedMs} ms, Δ={log.DeltaMs} ms) [{log.Drone}] {log.Message}"
            );
        }
    }

    public void PrintGroupedByDrone()
    {
        Console.WriteLine("\n\n===== RACE LOG (GROUPED BY DRONE) =====\n");

        var grouped = _entries
            .GroupBy(l => l.Drone)
            .OrderBy(g => g.Key);

        foreach (var droneGroup in grouped)
        {
            Console.WriteLine($"\n--- {droneGroup.Key} ---");

            foreach (var log in droneGroup.OrderBy(l => l.ElapsedMs))
            {
                Console.WriteLine(
                    $"{log.Timestamp:HH:mm:ss.fff} " +
                    $"(+{log.ElapsedMs} ms, Δ={log.DeltaMs} ms) {log.Message}"
                );
            }
        }    
    }
    public static void PrintRaceLog(RaceLog logger)
    {
        Console.WriteLine("\n\nPress L to see the racelog...");
        if (Console.ReadLine()?.ToUpper() == "L")
        {
            logger.PrintChronological();
            logger.PrintGroupedByDrone();
        }
    }
}