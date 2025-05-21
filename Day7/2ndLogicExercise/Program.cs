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
using System.Text;
using static System.Console;

class Program
{
    static void Main()
    {
        Clear();
        Print(135);
    }
    public static bool IsDivisibleBy(int nominator, int denominator)
    {
        return nominator % denominator == 0;
    }
    public static void Print(int nominator)
    {
        StringBuilder output = new StringBuilder();
        for (int i = 1; i <= nominator; i++)
        {
            if (IsDivisibleBy(i, 3))
            {
                output.Append("foo");
            }
            if (IsDivisibleBy(i, 5))
            {
                output.Append("bar");
            }
            if (IsDivisibleBy(i, 7))
            {
                output.Append("jazz");
            }
            if (!IsDivisibleBy(i, 3) && !IsDivisibleBy(i, 5) && !IsDivisibleBy(i, 7))
            {
                output.Append(i);
            }
            if (i != nominator)
            {
                output.Append(", ");
            }
            if (IsDivisibleBy(i, 7))
            {
                output.Append("\n");
            }
        }
        WriteLine(output.ToString());
    }
}