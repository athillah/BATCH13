using System.Reflection;

interface IShip
{
    public int Length { get; set; }
}

public class Battleship : IShip
{
    public int Length { get; set; } = 4;
}

public enum Orientation
{
    Horizontal,
    Vertical
}


class Program
{
    static void Main()
    {
        Battleship myBattleship = new Battleship();
        Console.WriteLine(myBattleship.Length);

        Console.WriteLine(GetOrientation());
    }
    public static string GetOrientation(Orientation orientation = Orientation.Horizontal)
    {
        return $"{orientation}";
    }
}