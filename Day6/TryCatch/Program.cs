using System;
using static System.Console;
using System.Text;

class Program
{
    static void Main()
    {
        int result = 0;
        WriteLine(TryDivide(10, 2, out result));
        WriteLine(result);

        IEnumerable<int> Fibs(int fibCount)
        {
            int prevFib = 0, curFib = 1;
            while (prevFib <= fibCount)
            {
                yield return prevFib;
                int newFib = prevFib + curFib;
                prevFib = curFib;
                curFib = newFib;
            }
        }

        StringBuilder fibs = new StringBuilder();
        foreach (int fib in Fibs(11))
        {
            fibs.Append(fib);
            fibs.Append(" ");
        }
        WriteLine(fibs.ToString());
    }
    public static bool TryDivide(int nominator, int denominator, out int result)
    {
        if (denominator == 0)
        {
            result = 0;
            return false;
        }
        else
        {
            result = nominator / denominator;
            return true;
        }
    }
}