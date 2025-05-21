using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Console;

// Belum ada implementasi yield return

public enum Axis { A, B, C, D, E, F, G, H }

public interface IPointOfInterest
{
    string? Tag { get; set; }
    Axis X { get; set; }
    Axis Y { get; set; }
}

public interface IMoveable
{
    void MoveHorizontal();
    void MoveVertical();
}

public class Player : IMoveable, IPointOfInterest
{
    public string? Tag { get; set; }
    public Axis X { get; set; }
    public Axis Y { get; set; }
    public Player(string tag)
    {
        Tag = tag;
        X = Axis.A;
        Y = Axis.A;
    }
    public void MoveHorizontal()
    {
        X = (Axis)((int)X + 1);
    }
    public void MoveVertical()
    {
        Y = (Axis)((int)Y + 1);
    }
}

public class Mapper
{
    private int _x { get; set; }
    private int _y { get; set; }
    public Mapper(int x, int y)
    {
        _x = x; _y = y;
    }
    public void Map(List<IPointOfInterest> PoIs)
    {
        StringBuilder map = new StringBuilder();
        for (int j = 0; j <= _y; j++)
        {
            for (int i = 0; i <= _x; i++)
            {
                var poi = PoIs.FirstOrDefault(p => p.X == (Axis)i && p.Y == (Axis)j);
                if (poi != null)
                {
                    map.Append($"[{poi.Tag}]");
                }
                else
                {
                    map.Append("[ ]");
                }
            }
            WriteLine(map.ToString());
            map.Clear();
        }
    }
}
class Program
{
    static void Main()
    {
        const int c = 8;
        Player PlayerA = new Player("A");
        Player PlayerB = new Player("B");
        Mapper MapperA = new Mapper(c, c);

        var points = new List<IPointOfInterest> { PlayerA, PlayerB };

        MapperA.Map(points);
        for (int i = 0; i <= c; i++)
        {
            Thread.Sleep(5000);
            Clear();
            PlayerA.MoveHorizontal();
            PlayerA.MoveVertical();
            PlayerB.MoveHorizontal();
            MapperA.Map(points);  
        }
    }
}