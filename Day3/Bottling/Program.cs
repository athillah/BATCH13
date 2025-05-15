using System;
using System.Drawing;
using static System.Console;
using System.Linq;

public enum Status {
    Idle,
    Queuing,
    Processing,
    Done
}

interface IProcessor {
    void Unload(int id);
    void PushIdle();
    void ProcessQueue();
    void Release();
    void Display();
    void Summary();
}

interface IProduct {
    public int Id { get; set; }
    public Status Status { get; set; }
}

public class Bottler : IProcessor {
    List<Bottle> Bottles = new List<Bottle>();
    public Bottler () {
        WriteLine("Bottler Generated");
    }
    public void Unload(int id)
    {
        Bottle bottle = new Bottle(id);
        Bottles.Add(bottle);
        WriteLine($"Got Bottle {bottle.Id}");
    }
    public void PushIdle() {
        
        NextProcess("push all", Status.Idle, Status.Queuing);
    }
    public void ProcessQueue() {
        if (IsProcessing())
        {
            WriteLine("Cant Do That. There is a process going on.");
        }
        else
        {
            WriteLine("Processing...");
            NextProcess("push one", Status.Queuing, Status.Processing);
            
        }
    }
    public void Release() {
        if (IsProcessing())
        {
            WriteLine("Releasing...");
            NextProcess("push one", Status.Processing, Status.Done);
        }
        else
        {
            WriteLine("Nothing to release...");
        }
    }
    public void NextProcess(string mode, Status initial, Status final) {
        if (mode == "push one") {
            foreach (Bottle bottle in Bottles) {
                if (bottle.Status == initial)
                {
                    bottle.Status = final;
                    WriteLine($"Bottle {bottle.Id} is now {bottle.Status}");
                    break;
                }
            }
        }
        if (mode == "push all") {
            foreach (Bottle bottle in Bottles) {
                if (bottle.Status == initial)
                {
                    bottle.Status = final;
                    WriteLine($"Bottle {bottle.Id} is now {bottle.Status}");
                }
            }
        }
    }
    public bool IsProcessing() {
        foreach (Bottle bottle in Bottles) {
            if (bottle.Status == Status.Processing){
                return true;
            }
        }
        return false;
    }
    public void Display() {
        foreach (Bottle bottle in Bottles) {
            WriteLine($"Bottle {bottle.Id} is {bottle.Status}");
        }
    }
    public void Summary() {
        var sum = Bottles
            .GroupBy(b => b.Status)
            .ToDictionary(bs => bs.Key, bs => bs.Count());
        foreach (Status status in Enum.GetValues(typeof(Status))) {
            int count = sum.ContainsKey(status) ? sum[status] : 0;
            Console.WriteLine($"{status}: {count}");
        }
    }
}

public class Bottle : IProduct {
    public int Id { get; set; }
    public Status Status { get; set; }
    public Bottle(int id) {
        Id = id;
        Status = Status.Idle;
    }
}

class Program {
    static void Main()
    {
        int id = 0;
        Bottler bottler = new Bottler();
        bool IsOver = false;
        while (!IsOver) {
            int Command = int.Parse(ReadLine());
            switch (Command)
            {
                case 1:
                    bottler.Unload(id);
                    id++;
                    break;
                case 2:
                    bottler.PushIdle();
                    break;
                case 3:
                    bottler.ProcessQueue();
                    break;
                case 4:
                    bottler.Release();
                    break;
                case 5:
                    bottler.Display();
                    break;
                case 6:
                    bottler.Summary();
                    break;
                case 7:
                    IsOver = true;
                    break;
                default:
                    WriteLine("Give me 1-7");
                    break;
            }
        }
    }
    void Feed() {

    }
}



