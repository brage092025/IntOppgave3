using System;

namespace DroneRace.Models;

public class DroneModel(string name, int maxCheckpoints, int delayMs)
{
    public string Name {get; set;} = name;
    public int MaxCheckpoints {get; set;} = maxCheckpoints; 
    public int DelayMs {get; set;} = delayMs; 
}

