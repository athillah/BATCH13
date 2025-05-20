using System;
using System.Collections.Generic;
using static System.Console;

public class SafetyOffEventArgs : EventArgs
{
    public string Warning { get; set; }
    public DateTime TimeOff { get; set; }
    public SafetyOffEventArgs(string warning, DateTime timeOff)
    {
        Warning = warning;
        TimeOff = timeOff;
    }
}

public interface ISensor
{
    string Tag { get; set; }
    string Warning { get; set; }
    int Data { get; set; }
    DateTime TimeOff { get; set; }
    void GetData();
    void OnSafetyOff(SafetyOffEventArgs e);
    event EventHandler<SafetyOffEventArgs> SafetyOff;
}

public interface IAlarm
{
    string Tag { get; set; }
    string AlarmOff { get; set; }
    event EventHandler<SafetyOffEventArgs> SafetyOff;
    void OnSafetyOff(object sender, SafetyOffEventArgs e);
}

public class Alarm : IAlarm
{
    public string Tag { get; set; }
    public string AlarmOff { get; set; }
    public event EventHandler<SafetyOffEventArgs> SafetyOff;
    public Alarm(string tag)
    {
        Tag = tag;
        AlarmOff = "WIUWIUWIWUWIWUIWUIWUIWUIWUIWUIWUWIWUIWU";
    }

    public void OnSafetyOff(object sender, SafetyOffEventArgs e)
    {
        WriteLine(AlarmOff);
    }
}

public class SmokeSensor : ISensor
{
    public string Tag { get; set; }
    public string Warning { get; set; }
    public int Data { get; set; }
    public DateTime TimeOff { get; set; }
    public event EventHandler<SafetyOffEventArgs> SafetyOff;

    public SmokeSensor(string tag)
    {
        Tag = tag;
        TimeOff = DateTime.Now;
        Warning = "There is a Smoke";
        Data = 0;
    }

    public void GetData()
    {
        Random rnd = new Random();
        Data = rnd.Next(20);
        WriteLine($"{Data} smoke on {Tag}");
        if (Data >= 13)
        {
            WriteLine("\nInvoking safety procedure...");
            OnSafetyOff(new SafetyOffEventArgs(Warning, TimeOff));
        }
    }

    public void OnSafetyOff(SafetyOffEventArgs e)
    {
        SafetyOff?.Invoke(this, e);
    }
}

public class SafetyMainStream
{
    public event EventHandler<SafetyOffEventArgs> SafetyOff;
    public void CoupleSensor(List<ISensor> sensors)
    {
        foreach (ISensor sensor in sensors)
        {
            sensor.SafetyOff += OnSafetyOff;
        }
    }

    public void CoupleAlarm(List<IAlarm> alarms)
    {
        foreach (IAlarm alarm in alarms)
        {
            SafetyOff += alarm.OnSafetyOff;
        }
    }

    public void CheckSensor(List<ISensor> sensors)
    {
        foreach (ISensor sensor in sensors)
        {
            sensor.GetData();
        }
    }

    public void OnSafetyOff(object sender, SafetyOffEventArgs e)
    {
        WriteLine($"\n\n[ALERT] {e.TimeOff} | {e.Warning}\n");
        SafetyOff?.Invoke(sender, e);
    }
}

public class Program
{
    public static void Main()
    {
        var sensors = new List<ISensor>
        {
            new SmokeSensor("Sensor A"),
            new SmokeSensor("Sensor B")
        };

        var SafetySystem = new SafetyMainStream();
        var alarm = new Alarm("alarm 1");
        var alarms = new List<IAlarm> { alarm };

        SafetySystem.CoupleAlarm(alarms);
        SafetySystem.CoupleSensor(sensors);
        for (int i = 0; i < 10; i++)
        {
            Clear();
            WriteLine($"{i+1} iteration {DateTime.Now}\n");
            SafetySystem.CheckSensor(sensors);
            Thread.Sleep(5000);
        }
    }
}