/* Write a simple console program that prints the number from 1 to n, for each number x:

print "foo", if x is divisible by 3
print "bar", if x is divisible by 5
print "foobar", if x is divisible by 3 and 5

print the number itself, if x satisfies none of the rule
Here's a sample output of such program with n=15
>> 1, 2, foo, 4, bar, foo, 7, 8, foo, bar, 11, foo, 13, 14,foobar */

using System;

namespace LogicExerciseDay1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pick a number:");
            int n = 15;
            /*int n = Convert.ToInt32(Console.Read());
            Console.WriteLine("N: " + n); */
            string output = "";
            for (int i=1; i<n+1; i++)
            {
                /*Console.WriteLine("COunting " + i);*/
                if (i%3==0)
                {
                    output += "foo";
                }
                if (i%5==0)
                {
                    output += "bar";
                }
                if (i%5!=0&&i%3!=0)
                {
                    output += i.ToString();
                }
                if (i != n)
                {
                    output += ", ";
                }
            }
            Console.WriteLine(output);
        }
    }
}
