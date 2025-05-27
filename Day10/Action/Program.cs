using System;

class Program
{
    static void Main()
    {
        Action actionlist = null;

        actionlist += mengaksi;

        actionlist?.Invoke();
    }

    static void mengaksi()
    {
        Console.WriteLine("Mengaksi");
    }
}