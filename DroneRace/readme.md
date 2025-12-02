# DRONERACE

### A – “Thread Race” (Thread + Join)

**Join vs. no join**

```csharp
//Wait for both threads to finish and join them back to main thread
        thread1.Join();
        thread2.Join();

        Console.WriteLine("Both Threads have finished operating!");

        Console.WriteLine("\n\nPress L to see the racelog...");
```

When joining the threads the remaining code waits for the threads to finish before executing, as opposed to just executing right away. 

The difference can be observed in the console output capturing the two drones(Eagle 500ms delay, and Hawk 250ms delay):

<details>
  <summary><i>Example: Comparison of outputs (Click to Expand)</i></summary>

| **With Join**                              | **Without Join**                            |
|---------------------------------------------|----------------------------------------------|
| Dronerace with Threads!                     | Dronerace with Threads!                      |
| Thread Eagle: Started...                    | 🟩 Both Threads have finished operating!      |
| Thread Hawk: Started...                     |                                              |
| Hawk has reached node 0 of 5...             | 🟦 Press L to see the racelog...              |
| Eagle has reached node 0 of 5...            | Thread Hawk: Started...                      |
| Hawk has reached node 1 of 5...             | Thread Eagle: Started...                     |
| Hawk has reached node 2 of 5...             | Hawk has reached node 0 of 5...              |
| Eagle has reached node 1 of 5...            | Hawk has reached node 1 of 5...              |
| Hawk has reached node 3 of 5...             | Eagle has reached node 0 of 5...             |
| Hawk has reached node 4 of 5...             | Hawk has reached node 2 of 5...              |
| Eagle has reached node 2 of 5...            | Eagle has reached node 1 of 5...             |
| Hawk has reached node 5 of 5...             | Hawk has reached node 3 of 5...              |
| Thread Hawk: Finished                       | Hawk has reached node 4 of 5...              |
| Eagle has reached node 3 of 5...            | Eagle has reached node 2 of 5...             |
| Eagle has reached node 4 of 5...            | Hawk has reached node 5 of 5...              |
| Eagle has reached node 5 of 5...            | Thread Hawk: Finished                        |
| Thread Eagle: Finished                      | Eagle has reached node 3 of 5...             |
| 🟩 Both Threads have finished operating!     | Eagle has reached node 4 of 5...             |
| 🟦 Press L to see the racelog...             | Eagle has reached node 5 of 5...             |
|                                             | Thread Eagle: Finished                       |

</details>

<br>

**Overloading shared resources**

We can see that console writeline, being a shared resource, is affected by the multiple threads. Not to a point of failure or that nodes are registred in an incorrect order, but we do see the drone order being affected. 

<details>
  <summary><i>Example with 3 drones, all with 250ms delay (Click to Expand)</i></summary>

<span style="color:red;">Thread Hawk: Started...</span>
<span style="color:blue;">Thread Falcon: Started...</span>
<span style="color:green;">Thread Eagle: Started...</span>
<span style="color:red;">Hawk has reached node 0 of 5...</span>
<span style="color:green;">Eagle has reached node 0 of 5...</span>
<span style="color:blue;">Falcon has reached node 0 of 5...</span>
<span style="color:blue;">Falcon has reached node 1 of 5...</span>
<span style="color:green;">Eagle has reached node 1 of 5...</span>
<span style="color:red;">Hawk has reached node 1 of 5...</span>
<span style="color:green;">Eagle has reached node 2 of 5...</span>
<span style="color:red;">Hawk has reached node 2 of 5...</span>
<span style="color:blue;">Falcon has reached node 2 of 5...</span>
<span style="color:green;">Eagle has reached node 3 of 5...</span>
<span style="color:red;">Hawk has reached node 3 of 5...</span>
<span style="color:blue;">Falcon has reached node 3 of 5...</span>
<span style="color:green;">Eagle has reached node 4 of 5...</span>
<span style="color:red;">Hawk has reached node 4 of 5...</span>
<span style="color:blue;">Falcon has reached node 4 of 5...</span>
<span style="color:green;">Eagle has reached node 5 of 5...</span>
<span style="color:green;">Thread Eagle: Finished</span>
<span style="color:red;">Hawk has reached node 5 of 5...</span>
<span style="color:red;">Thread Hawk: Finished</span>
<span style="color:blue;">Falcon has reached node 5 of 5...</span>
<span style="color:blue;">Thread Falcon: Finished</span>
</details>

<br>
When logging detailed timestamps we can also observe some variations from the set delay. Below a detailed log of 1 of 3 drones, running through 100 nodes with 0ms delay. We can see variations in several ms between some nodes.

<br>

<details>
  <summary><i>Example: Detailed log for drone Hawk (Click to Expand)</i></summary>

<pre>
> Timestamp (time elapsed, time elapsed since previous node) logevent
> 11:04:37.912 (+0 ms, Δ=0 ms) Started
> 11:04:37.935 (+28 ms, Δ=0 ms) Node 1
> 11:04:37.935 (+28 ms, Δ=28 ms) Node 0
> 11:04:37.937 (+30 ms, Δ=0 ms) Node 3
> 11:04:37.937 (+30 ms, Δ=2 ms) Node 2
> 11:04:37.938 (+31 ms, Δ=1 ms) Node 4
> 11:04:37.939 (+32 ms, Δ=1 ms) Node 5
> 11:04:37.940 (+33 ms, Δ=0 ms) Node 7
> 11:04:37.939 (+33 ms, Δ=1 ms) Node 6
> 11:04:37.941 (+34 ms, Δ=1 ms) Node 8
> 11:04:37.943 (+36 ms, Δ=2 ms) Node 9
> 11:04:37.951 (+45 ms, Δ=9 ms) Node 10
> 11:04:37.953 (+46 ms, Δ=1 ms) Node 11
> 11:04:37.954 (+47 ms, Δ=1 ms) Node 12
> 11:04:37.955 (+48 ms, Δ=1 ms) Node 13
> 11:04:37.958 (+51 ms, Δ=0 ms) Node 15
> 11:04:37.957 (+51 ms, Δ=3 ms) Node 14
> 11:04:37.960 (+53 ms, Δ=2 ms) Node 16
> 11:04:37.962 (+55 ms, Δ=2 ms) Node 17
> 11:04:37.969 (+62 ms, Δ=7 ms) Node 18
> 11:04:37.970 (+63 ms, Δ=1 ms) Node 19
> 11:04:37.971 (+64 ms, Δ=1 ms) Node 20
> 11:04:37.973 (+66 ms, Δ=2 ms) Node 21
> 11:04:37.975 (+68 ms, Δ=2 ms) Node 22
> 11:04:37.976 (+69 ms, Δ=1 ms) Node 23
> 11:04:37.981 (+74 ms, Δ=5 ms) Node 24
> 11:04:37.982 (+75 ms, Δ=1 ms) Node 25
> 11:04:37.983 (+76 ms, Δ=1 ms) Node 26
> 11:04:37.984 (+77 ms, Δ=1 ms) Node 27
> 11:04:37.985 (+78 ms, Δ=0 ms) Node 29
> 11:04:37.985 (+78 ms, Δ=1 ms) Node 28
> 11:04:37.987 (+80 ms, Δ=2 ms) Node 30
> 11:04:37.988 (+81 ms, Δ=1 ms) Node 31
> 11:04:37.989 (+82 ms, Δ=1 ms) Node 32
> 11:04:37.990 (+83 ms, Δ=1 ms) Node 33
> 11:04:37.992 (+85 ms, Δ=0 ms) Node 35
> 11:04:37.992 (+85 ms, Δ=2 ms) Node 34
> 11:04:37.993 (+86 ms, Δ=1 ms) Node 36
> 11:04:38.000 (+93 ms, Δ=7 ms) Node 37
> 11:04:38.001 (+94 ms, Δ=0 ms) Node 39
> 11:04:38.001 (+94 ms, Δ=1 ms) Node 38
> 11:04:38.002 (+95 ms, Δ=1 ms) Node 40
> 11:04:38.003 (+96 ms, Δ=0 ms) Node 42
> 11:04:38.003 (+96 ms, Δ=1 ms) Node 41
> 11:04:38.005 (+98 ms, Δ=2 ms) Node 43
> 11:04:38.007 (+100 ms, Δ=2 ms) Node 44
> 11:04:38.008 (+101 ms, Δ=0 ms) Node 46
> 11:04:38.007 (+101 ms, Δ=1 ms) Node 45
> 11:04:38.013 (+106 ms, Δ=5 ms) Node 47
> 11:04:38.016 (+109 ms, Δ=3 ms) Node 48
> 11:04:38.017 (+110 ms, Δ=1 ms) Node 49
> 11:04:38.018 (+111 ms, Δ=0 ms) Node 51
> 11:04:38.018 (+111 ms, Δ=1 ms) Node 50
> 11:04:38.019 (+112 ms, Δ=1 ms) Node 52
> 11:04:38.020 (+113 ms, Δ=0 ms) Node 54
> 11:04:38.020 (+113 ms, Δ=1 ms) Node 53
> 11:04:38.023 (+116 ms, Δ=3 ms) Node 55
> 11:04:38.024 (+117 ms, Δ=1 ms) Node 56
> 11:04:38.025 (+118 ms, Δ=0 ms) Node 58
> 11:04:38.024 (+118 ms, Δ=1 ms) Node 57
> 11:04:38.028 (+121 ms, Δ=3 ms) Node 59
> 11:04:38.034 (+127 ms, Δ=6 ms) Node 60
> 11:04:38.035 (+128 ms, Δ=0 ms) Node 62
> 11:04:38.034 (+128 ms, Δ=1 ms) Node 61
> 11:04:38.036 (+129 ms, Δ=1 ms) Node 63
> 11:04:38.037 (+130 ms, Δ=0 ms) Node 65
> 11:04:38.037 (+130 ms, Δ=1 ms) Node 64
> 11:04:38.038 (+131 ms, Δ=1 ms) Node 66
> 11:04:38.039 (+132 ms, Δ=1 ms) Node 67
> 11:04:38.041 (+134 ms, Δ=0 ms) Node 69
> 11:04:38.041 (+134 ms, Δ=2 ms) Node 68
> 11:04:38.042 (+135 ms, Δ=1 ms) Node 70
> 11:04:38.043 (+136 ms, Δ=1 ms) Node 71
> 11:04:38.046 (+139 ms, Δ=3 ms) Node 72
> 11:04:38.049 (+142 ms, Δ=3 ms) Node 73
> 11:04:38.050 (+143 ms, Δ=0 ms) Node 75
> 11:04:38.050 (+143 ms, Δ=1 ms) Node 74
> 11:04:38.051 (+145 ms, Δ=2 ms) Node 76
> 11:04:38.053 (+146 ms, Δ=0 ms) Node 78
> 11:04:38.053 (+146 ms, Δ=1 ms) Node 77
> 11:04:38.054 (+147 ms, Δ=1 ms) Node 79
> 11:04:38.055 (+148 ms, Δ=0 ms) Node 81
> 11:04:38.055 (+148 ms, Δ=1 ms) Node 80
> 11:04:38.057 (+150 ms, Δ=2 ms) Node 82
> 11:04:38.058 (+151 ms, Δ=0 ms) Node 84
> 11:04:38.058 (+151 ms, Δ=1 ms) Node 83
> 11:04:38.059 (+152 ms, Δ=0 ms) Node 86
> 11:04:38.059 (+152 ms, Δ=1 ms) Node 85
> 11:04:38.066 (+159 ms, Δ=7 ms) Node 87
> 11:04:38.067 (+160 ms, Δ=1 ms) Node 88
> 11:04:38.072 (+165 ms, Δ=5 ms) Node 89
> 11:04:38.074 (+167 ms, Δ=2 ms) Node 90
> 11:04:38.075 (+168 ms, Δ=1 ms) Node 91
> 11:04:38.076 (+169 ms, Δ=1 ms) Node 92
> 11:04:38.081 (+174 ms, Δ=5 ms) Node 93
> 11:04:38.083 (+176 ms, Δ=2 ms) Node 94
> 11:04:38.085 (+178 ms, Δ=2 ms) Node 95
> 11:04:38.086 (+179 ms, Δ=1 ms) Node 96
> 11:04:38.087 (+180 ms, Δ=1 ms) Node 97
> 11:04:38.089 (+182 ms, Δ=2 ms) Node 98
> 11:04:38.091 (+184 ms, Δ=2 ms) Node 99
> 11:04:38.093 (+186 ms, Δ=2 ms) Node 100
> 11:04:38.097 (+190 ms, Δ=4 ms) Finished
</pre>
</details>
<br>

**Observations about shared resources:**
- Console.WriteLine output is interleaved due to concurrent access.
- The order of drone updates is not deterministic.
- No nodes are lost, but timestamps can vary slightly due to scheduling.
- `Join` ensures that the main thread waits for all drones to finish before continuing.
- Without `Join`, subsequent code executes immediately, and output may appear jumbled.
- Multi-threaded access to shared resources like Console can interleave outputs, even if the logic is correct.
- Timing variations are expected, even with minimal delays, due to OS scheduling and context switching.


### B - “Tower says go!” (Task + TaskCompletionSource)

**Simulated failures**

I added two simulated errors - a Fuel error that will trigger if a nodes random extra delay brings the total delay below 0, and an Engine error that has a 2% probability of triggering per node the drone passes:

<details><summary><i>Fuel error code example (Click to expand)</i></summary>

```csharp
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
```

</details>
<br>
<details><summary><i>Engine error code example (Click to expand)</i></summary>

```csharp
// --- Simulated Failure: Engine failure ---
if (_rnd.NextDouble() < 0.02) // 2% chance of engine failure
{
    log.Log(drone.Name, $"ENGINE FAILURE at node {currentNode}!");

    tsc.SetException(new InvalidOperationException($"{drone.Name} ENGINE FAILURE at node {currentNode}!"));
    return;
}

log.Log(drone.Name, $"Resuming after {totalDelay}ms delay at node {currentNode}");
```

</details>
<br>

### C - “Async orchestration” (Async/Await)

Keeping the same simulated failures as in section B.

Structure is much cleaner and more logical. A lot less "boilerplate" code which makes it easier to maintain.


### D - “Control Tower API” (HttpClient with Async/Await)

I chose to keep it simple and go for option one - where I just consume an API and map it to some relevant factors. Using AsyncRace as the startingpoint for the code. 

I went with https://jsonplaceholder.typicode.com/albums which contains 100 different albums in this JSON format:
```JSON
{
  "userId": 1,
  "id": 1,
  "title": "quidem molestiae enim"
}
```
So I pick a random album 1-100:
```csharp
var rndAlbumId = new Random().Next(1, 100);
        var response = await _http.GetAsync($"https://jsonplaceholder.typicode.com/albums/{rndAlbumId}");
```
the "id" is used to set the number of nodes/checkpoints for the drones, and the number of words in the "albumId" is used to simulate wind conditions that add additional delay:

```csharp
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
    }
```

### Final thougths

**What did you observe when ``Join`` was removed in section A, and why?**
`Join` ensures that the main thread waits for all drones to finish before continuing.
So without `Join` the subsequent code executed immediately, and console output was jumbled (more details in section A).

**Compare ``Thread/Join``, ``Task/TCS`` and ``Async/Await`` with regards to control, resource usage, error handling and readability**

|                    | **Thread / Join**                                                                                                           | **Task + TaskCompletionSource**                                                                                           | **Async / Await**                                                                                                       |
| ------------------ | --------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------- |
| **Control**        | Very explicit and low-level. You manage threads manually and coordinate completion with `Join()`. Limited, rigid signaling. | Fine-grained but extremely manual. You control completion (`SetResult`), error propagation, and continuations explicitly. | High-level control with minimal effort. Natural flow, clean orchestration (`await`, `WhenAll`), compiler handles state. |
| **Resource usage** | Heavy: one OS thread per drone, blocking with `Sleep`, poor scalability.                                                    | Light: uses thread pool + non-blocking waits, but still manual logic.                                                     | Very efficient: async frees threads during waits, supports massive concurrency.                                         |
| **Error handling** | Primitive: exceptions in threads don’t propagate; must capture manually.                                                    | Structured but manual: must call `SetException` and manage propagation yourself.                                          | Automatic: exceptions bubble naturally; `try/catch` integrates seamlessly with async.                                   |
| **Readability**    | Moderate: simple concepts but low-level and verbose. Blocking logic reduces clarity.                                        | Low: nested callbacks, boilerplate, continuations make code harder to read.                                               | Excellent: looks synchronous, minimal boilerplate, easiest to maintain.                                                 |
