using DroneRace.Models;

namespace DroneRace.Services;

public static class WeatherMapper
{
    // Maps API data to simulate weather conditions affecting the race
    public static (int nodes, int weatherDelay) Map(ApiData api)
    {
        int nodes = api.Id; // Use album ID as number of nodes

        int wordCount = api.Title?
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Length ?? 1;

        int weatherDelay = wordCount switch // Simple delay based on word count of title
        {
            <= 2 => 0,       // Word count <= 2, weather is Calm, 0 ms additional delay
            <= 4 => 200,     // Word count 3-4, weather is Windy, 200 ms additional delay
            _    => 600      // Word count > 4, weather is Stormy, 600 ms additional delay
        };

        return (nodes, weatherDelay);
    }
}
