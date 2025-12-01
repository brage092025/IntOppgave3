using System;
using DroneRace.Models;

namespace DroneRace.Services;

public class ThreadRace
{ 
    public static void StartThreadRace(RaceLog log)
    {
        Console.WriteLine("Dronerace with Threads and Join!\n\n");

        // First drone and thread setup
        var drone1 = new DroneModel("Hawk", 5, 250);
        var thread1 = new Thread(ThreadCounter);

        // Second drone and thread setup
        var drone2 = new DroneModel("Eagle", 5, 500);
        var thread2 = new Thread(ThreadCounter);

        /* // Third drone and thread setup for stress testing Console output
        var drone3 = new DroneModel("Falcon", 100, 0);
        var thread3 = new Thread(ThreadCounter); 
  */

        thread1.Start((log, drone1));
        thread2.Start((log, drone2));
        /* thread3.Start((log, drone3)); */

        //Wait for both threads to finish and join them back to main thread
        thread1.Join();
        thread2.Join();
/*         thread3.Join(); 
 */
        Console.WriteLine("All drones have finished!");

    }


    private static void ThreadCounter(object? state)

    {
        var (log, drone) = ((RaceLog, DroneModel))state!;

        log.Log(drone.Name, "Started");
        Console.WriteLine($"Thread {drone.Name}: Started...");

        for (int node = 0; node <= drone.MaxCheckpoints; node++)
        {
            Thread.Sleep(drone.DelayMs);
            log.Log(drone.Name, $"Node {node}");
            Console.WriteLine($"{drone.Name} has reached node {node} of {drone.MaxCheckpoints}");
        }

        log.Log(drone.Name, "Finished");
        Console.WriteLine($"Thread {drone.Name}: Finished");
    }
}