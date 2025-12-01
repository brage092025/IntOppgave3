using System;

namespace DroneRace.Interfaces;

public interface IRaceLog
{
    void Log(string droneName, string message);
    IReadOnlyList<LogEntry> GetLogs();
}

public class LogEntry
{
    public DateTime Timestamp { get; init; }
    public long ElapsedMs { get; init; }
    public long DeltaMs { get; init; }     
    public string Drone { get; init; } = "";
    public string Message { get; init; } = "";
}
