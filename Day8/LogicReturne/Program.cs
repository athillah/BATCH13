using System;

class LogicReturner
{
    bool Return { get; set; }
    int Iteration { get; set; }
    public void Iterate(int turns)
    {
        for (int i = 0; i <= turns; i++)
        {
            Iteration = i;
            Console.WriteLine($"Turn:{Return}");
            Return = ReturningLogic();
        }
    }
    public bool ReturningLogic()
    {
        Console.WriteLine($"Iteration: {Iteration}");
        return Return ? false : true;
    }
}

public class Program
{
    public static void Main()
    {
        LogicReturner TurnTaker = new LogicReturner();
        TurnTaker.Iterate(20);
    }
}