using System.Reflection;

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coordinate(int x, int y)
    {
        X = x; Y = y;
    }
}

class Program
{
    static void Main()
    {
        List<Coordinate> coordinates = new List<Coordinate>();
        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 8; j++)
            {
                coordinates.Add(new Coordinate(i, j));
            }
        }
        foreach (var coordinate in coordinates)
        {
            Console.WriteLine($"X: {coordinate.X}, Y: {coordinate.Y}");
        }
    }
}