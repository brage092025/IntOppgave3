using DroneRace.Services;

var log = new RaceLog();


Console.WriteLine("A: Thread Race (Thread + Join)");
Console.WriteLine("B: Tower says go! (Task + TaskCompletionSource)");
Console.WriteLine("C: Async orchestration (Async/Await)");
Console.WriteLine("D: Control Tower API (HttpClient + Async/Await)");
Console.WriteLine("E: Exit");
Console.Write("Choose an option: ");

var menuSelection = Console.ReadLine()?.ToUpper();

    switch (menuSelection)
    {
        case "A":
            ThreadRace.StartThreadRace(log);
            break;
        case "B":
            try
            {
                await TaskRace.StartTaskRace(log);
                Console.WriteLine("\nRace finished successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n=== RACE FAILURE DETECTED ===");
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }
            break;
        case "C":
            await AsyncRace.StartAsyncRace(log);
            break;
        case "D":
            var api = new ApiDataService(new HttpClient());
            var race = new ApiRace(api);
            await race.StartApiRace(log);
            break;
        case "E":
            Console.WriteLine("Exiting program.");
            return;
        default:
            Console.WriteLine("Invalid selection. Please try again.");
            break;
    }
    
RaceLog.PrintRaceLog(log);

   
