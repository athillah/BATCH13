using System;
using static System.Console;

//overload
public class Print {
    public Print (int a) {
        WriteLine ($"int: {a}");
    }
    public Print (string a) {
        WriteLine ($"string: {a}");
    }
    public Print (string a, int b) {
        WriteLine ($"string: {a}, int: {b}");
    }
}

//Indexers
public class Indexer {
    private string[] words = "Lorem ipsum dolor sit amet".Split(' ');
    public string this[int index] {
        get { return words[index]; }
        set { words[index] = value; }
    }
}

//Inheritance
public class Robot {
    public string Name;
}

public class Humanoid : Robot {
    public int Legs;
}

public class Drone : Robot {
    public int Motors;
}

//using objects
public class Queue
{
    private int _position = 0;
    private object[] _items = new object[10];
    public void Enqueue(object item) {
        _items[_position] = item;
        WriteLine($"Enqueue: {item}");
        _position++;
    }
    public object Dequeue() {
        _position--;
        WriteLine($"Dequeue: {_items[_position]}");
        return _items[_position];
    }
    public void Show() {
        for (int i = 0; i < _position; i++) {
            WriteLine(_items[i]);
        }
    }
}

//Struct
public struct Clothe {
    public string Name { get; set; }
    private double _basePrice { get; set; }
    public Clothe(string name, double price) {
        Name = name;
        _basePrice = price;
    }
    public double GetPrice() {
        Random priceModifier = new Random();
        double modifier = 0.8 + priceModifier.NextDouble() * (1.2 - 0.8);
        return _basePrice * modifier;
    }
    public double ApplyDiscount(double discount) {
        return _basePrice - _basePrice * discount;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        /*
        //overload
        WriteLine("Overload");
        Print p1 = new Print(1);
        Print p2 = new Print("Hello");
        Print p3 = new Print("Hello", 1);

        //indexers
        WriteLine("\n Indexers");
        Indexer indexer = new Indexer();
        WriteLine(indexer[0]);
        indexer[0] = "Hello";
        WriteLine(indexer[0]);
        WriteLine(indexer[1]);

        //inheritance polymorphism
        WriteLine("\n Inheritance polymorphism");

        Humanoid humanoid = new Humanoid { Name = "Robot", Legs = 2 };
        Drone drone = new Drone { Name = "Drone", Motors = 4 };
        Display(humanoid);
        Display(drone);

        //downcasting
        WriteLine("\nDowncasting with is");
        Robot RobotHumanoid = new Humanoid { Name = "Robot", Legs = 2 };
        if (RobotHumanoid is Humanoid humanoid1)
            WriteLine($"Humanoid with {humanoid1.Legs} legs");
        else
            WriteLine("Not a humanoid");

        WriteLine("\nDowncasting with as");
        Robot RobotDrone = new Drone { Name = "Drone", Motors = 4 };
        Drone drone1 = RobotDrone as Drone;
        if (drone1 != null)
            WriteLine($"Drone with {drone1.Motors} motors");
        else
            WriteLine("Not a drone");

        //using objects
        WriteLine("\nUsing objects");
        Queue queue = new Queue();
        queue.Enqueue(1);
        queue.Enqueue("Hello");
        queue.Enqueue(new Robot { Name = "Robot" });
        queue.Enqueue(new Humanoid { Name = "Humanoid", Legs = 2 });
        queue.Enqueue(new Drone { Name = "Drone", Motors = 4 });
        queue.Show();
        queue.Dequeue();
        queue.Dequeue();
        queue.Dequeue();
        queue.Dequeue();
        queue.Dequeue();
        queue.Show();

        //boxing and unboxing
        WriteLine("\nBoxing and unboxing");
        int number = 1;
        WriteLine($"Number: {number}. Type");
        object boxedNumber = number; // Boxing
        WriteLine($"Boxed Number: {boxedNumber}");
        int unboxedNumber = (int)boxedNumber; // Unboxing
        WriteLine($"Unboxed Number: {unboxedNumber}");

        //equals
        WriteLine("\nEquals");
        object obj1 = new object();
        object obj2 = new object();
        WriteLine($"obj1 == obj2: {obj1 == obj2}");
        WriteLine($"obj1.Equals(obj2): {obj1.Equals(obj2)}");
        WriteLine($"obj1.GetHashCode(): {obj1.GetHashCode()}");
        WriteLine($"obj2.GetHashCode(): {obj2.GetHashCode()}");
        */

        Clothe clothe = new Clothe("T-shirt", 100);
        WriteLine($"Clothe: {clothe.Name}, Price: {clothe.GetPrice()}, Discounted Price: {clothe.ApplyDiscount(0.25)}");
    }

    public static void Display(Robot robot)
    {
        WriteLine($"{robot.Name}");
    }
}