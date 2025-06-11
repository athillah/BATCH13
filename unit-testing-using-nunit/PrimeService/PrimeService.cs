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
            throw new NotImplementedException("Method not implemented yet");
        }

        public int GetNextPrime(int number)
        {
            throw new NotImplementedException("Method not implemented yet");
        }
    }
}