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
    static bool flag = false;
    static void Main()
    {
        Clear();
        Print(135);
    }
    public static void output(StringBuilder sb, string input)
    {
        sb.Append(input);
        flag = true;
    }
    public static void Print(int nominator)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i <= nominator; i++)
        {
            flag = false;
            if (i % 3 == 0) output(sb, "foo");
            if (i % 4 == 0) output(sb, "baz");
            if (i % 5 == 0) output(sb, "bar");
            if (i % 7 == 0) output(sb, "jazz");
            if (i % 9 == 0) output(sb, "huzz");
            if (!flag) output(sb, i.ToString());
            if (i != nominator) output(sb, ", ");
            if (i % 10 == 0) output(sb, "\n");
        }
        WriteLine(sb.ToString());
    }
}