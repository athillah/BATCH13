// print "foo", if x is divisible by 3
// print "bar", if x is divisible by 5
// print "foobar", if x is divisible by 3 and 5

// print the number itself, if x satisfies none of the rule
// Here's a sample output of such program with n=15
// >> 1, 2, foo, 4, bar, foo, 7, 8, foo, bar, 11, foo, 13, 14,foobar

// Continuing on the previous question. Add the following rules
// print "jazz", if x is divisible by 7
// This means for x = 21 x = 35 and x = 105 the program should print "foojazz", "barjazz" and "foobarjazz" respectively

using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static System.Console;

interface IMyClass
{
    void AddRule(int input, string output);
}

public class MyClass : IMyClass
{
    private Dictionary<int, string> _rule { get; set; }
    public MyClass()
    {
        _rule = new Dictionary<int, string>();
    }
    public void AddRule(int input, string output)
    {
        _rule[input] = output;
        _rule = _rule.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }
    public Dictionary<int, string> GetRule()
    {
        return _rule;
    }
    public int AddRuleInt()
    {
        Console.Write("Input a number to be a division rule: ");
        int input;
        while (!int.TryParse(Console.ReadLine(), out input) || input <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
        return input;
    }
    public string AddRuleString()
    {
        Console.Write("Input a string to override number that divised by number above: ");
        string output;
        while (string.IsNullOrWhiteSpace(output = Console.ReadLine()))
        {
            Console.WriteLine("Invalid input. Please enter right string.");
        }
        return output;
    }
    public StringBuilder Print(int nominator)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i <= nominator; i++)
        {
            bool flag = false;
            foreach (var rule in _rule)
            {
                if (i % rule.Key == 0)
                {
                    sb.Append(rule.Value);
                    flag = true;
                }
            }
            if (!flag) sb.Append(i.ToString());
            if (i != nominator) sb.Append(", ");
        }
        return sb;
    }
}

class Program
{
    static bool flag = false;
    static void Main()
    {
        MyClass myClass = new MyClass();
        while (true)
        {
            Clear();
            Console.Clear();
            Console.Write("==Logician==\n 1. Print\n 2. Add Rule\n 3. Rule list\n 4. Exit\n Input: ");
            int menu;
            int.TryParse(Console.ReadLine(), out menu);
            switch (menu)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("==Print==");
                    while (true)
                    {
                        Console.Write("Input how much iterations you want to print: ");
                        int nominator;
                        if (int.TryParse(Console.ReadLine(), out nominator) && nominator > 0)
                        {
                            StringBuilder sb = myClass.Print(nominator);
                            Console.Clear();
                            Console.WriteLine("===Output===");
                            Console.WriteLine(sb.ToString());
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive integer.");
                        }
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("===Add Rule===");
                    int input = myClass.AddRuleInt();
                    string output = myClass.AddRuleString();
                    myClass.AddRule(input, output);
                    Console.WriteLine($"Rule added: number that is divisible by {input} will be replaced with '{output}");
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("===Rule List===");
                    Console.WriteLine("Division Rule\t\tOutput");
                    Console.WriteLine("-------------------------");
                    foreach (var rule in myClass.GetRule())
                    {
                        Console.WriteLine($"{rule.Key}\t{rule.Value}");
                    }
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}