using System;
using System.IO;
using System.Text;
using System.Threading;

public class Updater
{
    private TimeOnly _currentTime { get; set; }
    public Updater()
    {
        _currentTime = TimeOnly.FromDateTime(DateTime.Now);
    }
    public string GetTime()
    {
        return _currentTime.ToString("HH:mm:ss");
    }
    public void SaveFile(string message)
    {
        using (FileStream fs = new FileStream("log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
        {
            writer.WriteLine();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                _currentTime = TimeOnly.FromDateTime(DateTime.Now);
                if (i % 3 == 0) writer.Write("bonjour ");
                if (i % 5 == 0) writer.Write("ni hao ");
                if (i % 7 == 0) writer.Write("morgen ");
                writer.WriteLine(_currentTime);
            }
            writer.WriteLine(message);
        }
    }
}

class Program
{
    static void Main()
    {
        Updater MyUpdater = new Updater();
        int y = 50;
        for (int i = 0; i < y; i++)
        {
            Thread.Sleep(100);
            if (i % 5 == 0) MyUpdater.SaveFile(i.ToString());
        }
    }
}