using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using static System.Console;

delegate double Calculation(double x, double y);

class Program
{
	static double Scalar(double x, double y) => x * y;
    static double Divide(double x, double y) => x / y;

	static void Main()
    {
        Calculation Pressure = Divide;
        Calculation Force = Scalar;
        Calculation Area = Divide;
        double force = 0;
        double area = 0;
        double pressure = 0;
        WriteLine("What do you want to find?\n 1) Pressure\n 2) Force\n 3) Area");
        int Com = int.Parse(ReadLine());
        switch (Com)
        {
            case 1:
                WriteLine("Input Force");
                force = double.Parse(ReadLine());
                WriteLine("Input Area");
                area = double.Parse(ReadLine());
                WriteLine($"Pressure: {Pressure(force, area)}");
                break;
            case 2:
                WriteLine("Input Pressure");
                pressure = double.Parse(ReadLine());
                WriteLine("Input Area");
                area = double.Parse(ReadLine());
                WriteLine($"Force: {Force(pressure, area)}");
                break;
            case 3:
                WriteLine("Input Pressure");
                pressure = double.Parse(ReadLine());
                WriteLine("Input Force");
                force = double.Parse(ReadLine());
                WriteLine($"Area: {Area(force, pressure)}");
                break;
            default:
                WriteLine("Wrong Input");
                break;
        }
    }
}