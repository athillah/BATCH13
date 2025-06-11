namespace PrimeService
{
    public class PrimeService
    {
        public bool IsPrime(int candidate)
        {
            if (candidate == 1)
                return false;

            if (candidate == 2)
                return true;

            if (candidate % 2 == 0)
                return false;

            if (candidate < 0)
                return false;

            if (candidate == 3)
                return true;

            if (candidate % 3 == 0)
                return false;

            int limit = (int)Math.Sqrt(candidate);
            for (int divisor = 5; divisor <= limit; divisor += 6)
                if (candidate % divisor == 0 || candidate % (divisor + 2) == 0)
                    return false;

            return true;
        }

        public int[] FindPrimesUpTo(int limit)
        {
            var result = new List<int>();

            for (int i = 0; i <= limit; i++)
                if (IsPrime(i))
                    result.Add(i);

            return result.ToArray();
        }

        public int GetNextPrime(int number)
        {
            int i = 2;

            if (number <= 1)
                return i;
            
            int limit = number + 10;
            bool flag = false;
            for (i = 2; i <= limit; i++)
            {
                if (IsPrime(i) && flag)
                    return i;
                if (i == number)
                    flag = true;
            }
            return 0;
        }
    }
}