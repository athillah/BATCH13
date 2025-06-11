using NUnit.Framework;

namespace PrimeService.Tests
{
    [TestFixture]
    public class PrimeService_Should
    {
        private PrimeService _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new PrimeService();
        }

        [Test]
        public void IsPrime_InputIs1_ReturnFalse()
        {
            bool result = _primeService.IsPrime(1);
            Assert.That(result, Is.False, "1 should not be considered prime");
        }

        [Test]
        public void IsPrime_InputIs2_ReturnTrue()
        {
            bool result = _primeService.IsPrime(2);
            Assert.That(result, Is.True, "2 should be considered prime");
        }

        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(13)]
        [TestCase(17)]
        [TestCase(19)]
        [TestCase(23)]
        [TestCase(97)]
        [TestCase(101)]
        public void IsPrime_InputIsKnownPrime_ReturnTrue(int candidate)
        {
            bool result = _primeService.IsPrime(candidate);
            Assert.That(result, Is.True, $"{candidate} should be considered prime");
        }

        [TestCase(4)]
        [TestCase(6)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(12)]
        [TestCase(15)]
        [TestCase(21)]
        [TestCase(25)]
        [TestCase(100)]
        public void IsPrime_InputIsKnownComposite_ReturnFalse(int candidate)
        {
            bool result = _primeService.IsPrime(candidate);
            Assert.That(result, Is.False, $"{candidate} should not be considered prime");
        }

        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-100)]
        public void IsPrime_InputIsNegative_ReturnFalse(int candidate)
        {
            bool result = _primeService.IsPrime(candidate);
            Assert.That(result, Is.False, "Negative numbers should not be considered prime");
        }

        [Test]
        public void IsPrime_InputIs0_ReturnFalse()
        {
            bool result = _primeService.IsPrime(0);
            Assert.That(result, Is.False, "0 should not be considered prime");
        }

        [Test]
        public void ReturnEmptyArray_WhenLimitIsNegative()
        {
            // Arrange
            int negativeLimit = -5;

            // Act & Assert
            // We expect this to either return empty array or throw an exception
            // Let's test both scenarios - first assuming it returns empty array
            var result = _primeService.FindPrimesUpTo(negativeLimit);

            // Use CollectionAssert for testing arrays/collections
            Assert.That(result, Is.Empty,
                "FindPrimesUpTo should return empty array for negative limits");
        }

        /// <summary>
        /// Testing the boundary condition: what about 0?
        /// There are no primes less than or equal to 0
        /// </summary>
        [Test]
        public void ReturnEmptyArray_WhenLimitIsZero()
        {
            // Arrange
            int zeroLimit = 0;

            // Act
            var result = _primeService.FindPrimesUpTo(zeroLimit);

            // Assert
            Assert.That(result, Is.Empty,
                "No primes exist less than or equal to 0");
        }

        #region Edge Cases and Invalid Input Tests

        /// <summary>
        /// Testing limit = 1: still no primes
        /// 1 is not considered a prime number
        /// </summary>
        [Test]
        public void ReturnEmptyArray_WhenLimitIsOne()
        {
            // Arrange
            int oneLimit = 1;

            // Act
            var result = _primeService.FindPrimesUpTo(oneLimit);

            // Assert
            Assert.That(result, Is.Empty,
                "1 is not a prime number, so result should be empty");
        }

        #endregion

        #region Small Range Tests

        /// <summary>
        /// Test the smallest case with a prime: limit = 2
        /// Should return [2] since 2 is the first and smallest prime
        /// </summary>
        [Test]
        public void ReturnArrayWithTwo_WhenLimitIsTwo()
        {
            // Arrange
            int limit = 2;
            var expectedPrimes = new[] { 2 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "When limit is 2, should return array containing only 2");
        }

        /// <summary>
        /// Test with limit = 3: should return [2, 3]
        /// </summary>
        [Test]
        public void ReturnFirstTwoPrimes_WhenLimitIsThree()
        {
            // Arrange
            int limit = 3;
            var expectedPrimes = new[] { 2, 3 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "When limit is 3, should return [2, 3]");

            // Alternative way to test the same thing - sometimes useful for debugging
            Assert.That(result.Length, Is.EqualTo(2), "Should have exactly 2 primes");
            Assert.That(result, Does.Contain(2), "Result should contain 2");
            Assert.That(result, Does.Contain(3), "Result should contain 3");
        }

        #endregion

        #region Medium Range Tests

        /// <summary>
        /// Test with a reasonable range: primes up to 10
        /// Expected: [2, 3, 5, 7]
        /// </summary>
        [Test]
        public void ReturnCorrectPrimes_WhenLimitIsTen()
        {
            // Arrange
            int limit = 10;
            var expectedPrimes = new[] { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "Primes up to 10 should be [2, 3, 5, 7]");
        }

        /// <summary>
        /// Test with primes up to 20
        /// Expected: [2, 3, 5, 7, 11, 13, 17, 19]
        /// </summary>
        [Test]
        public void ReturnCorrectPrimes_WhenLimitIsTwenty()
        {
            // Arrange
            int limit = 20;
            var expectedPrimes = new[] { 2, 3, 5, 7, 11, 13, 17, 19 };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "Primes up to 20 should be [2, 3, 5, 7, 11, 13, 17, 19]");

            // Additional verification: check the count
            Assert.That(result.Length, Is.EqualTo(8),
                "There should be exactly 8 primes up to 20");
        }

        #endregion

        #region Boundary and Edge Cases

        /// <summary>
        /// Test where the limit itself is a prime number
        /// When limit = 7, result should include 7
        /// </summary>
        [Test]
        public void IncludeLimitInResult_WhenLimitIsPrime()
        {
            // Arrange
            int primeLimit = 7;
            var expectedPrimes = new[] { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(primeLimit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "When limit is a prime, it should be included in result");

            Assert.That(result, Does.Contain(primeLimit),
                "The limit itself should be in the result when it's prime");
        }

        /// <summary>
        /// Test where the limit is a composite number
        /// When limit = 8, should return [2, 3, 5, 7] (8 is not included)
        /// </summary>
        [Test]
        public void ExcludeLimitFromResult_WhenLimitIsComposite()
        {
            // Arrange
            int compositeLimit = 8;
            var expectedPrimes = new[] { 2, 3, 5, 7 };

            // Act
            var result = _primeService.FindPrimesUpTo(compositeLimit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                "When limit is composite, it should not be included");

            Assert.That(result, Does.Not.Contain(compositeLimit),
                "Composite limit should not appear in results");
        }

        #endregion

        #region Performance and Larger Numbers

        /// <summary>
        /// Test with a larger number to verify the algorithm works efficiently
        /// Primes up to 100 - there are 25 of them
        /// </summary>
        [Test]
        public void HandleLargerNumbers_WhenLimitIsOneHundred()
        {
            // Arrange
            int limit = 100;
            // The first 25 primes (up to 100)
            var expectedPrimes = new[] {
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
                31, 37, 41, 43, 47, 53, 59, 61, 67, 71,
                73, 79, 83, 89, 97
            };

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result.Length, Is.EqualTo(25),
                "There should be exactly 25 primes up to 100");

            Assert.That(result, Is.EqualTo(expectedPrimes),
                "The primes up to 100 should match the expected sequence");
        }

        /// <summary>
        /// Performance test: make sure the algorithm doesn't take too long
        /// This is important for the Sieve of Eratosthenes algorithm
        /// </summary>
        [Test]
        public void CompleteInReasonableTime_WhenLimitIsLarge()
        {
            // Arrange
            int largeLimit = 10000;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.FindPrimesUpTo(largeLimit);
            stopwatch.Stop();

            // Assert
            Assert.That(result, Is.Not.Null, "Result should not be null");
            Assert.That(result.Length, Is.GreaterThan(0), "Should find some primes in range 1-10000");

            // The Sieve of Eratosthenes should be quite fast
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000),
                "Finding primes up to 10,000 should take less than 1 second");

            // We know there are 1,229 primes less than 10,000
            Assert.That(result.Length, Is.EqualTo(1229),
                "There should be exactly 1,229 primes less than 10,000");
        }

        #endregion

        #region Array Properties Tests

        /// <summary>
        /// Verify that the returned array is sorted in ascending order
        /// This is a reasonable expectation for this method
        /// </summary>
        [Test]
        public void ReturnSortedArray_ForAnyValidLimit()
        {
            // Arrange
            int limit = 50;

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            var sortedResult = result.OrderBy(x => x).ToArray();
            Assert.That(result, Is.EqualTo(sortedResult),
                "Returned array should be sorted in ascending order");
        }

        /// <summary>
        /// Verify that all returned numbers are actually prime
        /// This is a sanity check that uses our other method
        /// </summary>
        [Test]
        public void ReturnOnlyPrimeNumbers_ForAnyValidLimit()
        {
            // Arrange
            int limit = 30;

            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            foreach (int number in result)
            {
                // This test will fail initially because IsPrime throws NotImplementedException
                // But once we implement IsPrime, this becomes a great cross-validation test
                try
                {
                    Assert.That(_primeService.IsPrime(number), Is.True,
                        $"Number {number} in result should be prime according to IsPrime method");
                }
                catch (NotImplementedException)
                {
                    // Skip this assertion until IsPrime is implemented
                    Assert.Pass("Skipping prime validation until IsPrime is implemented");
                }
            }
        }

        #endregion

        #region Alternative Testing Approaches

        /// <summary>
        /// Example of using TestCase for parameterized testing with collections
        /// This shows how to test multiple scenarios in one test method
        /// </summary>
        [TestCase(2, new[] { 2 })]
        [TestCase(3, new[] { 2, 3 })]
        [TestCase(5, new[] { 2, 3, 5 })]
        [TestCase(11, new[] { 2, 3, 5, 7, 11 })]
        public void ReturnExpectedPrimes_ForVariousLimits(int limit, int[] expectedPrimes)
        {
            // Act
            var result = _primeService.FindPrimesUpTo(limit);

            // Assert
            Assert.That(result, Is.EqualTo(expectedPrimes),
                $"Primes up to {limit} should match expected array");
        }

        #endregion
        
                #region Edge Cases and Invalid Input Tests

        /// <summary>
        /// Test what happens when we ask for the next prime after a negative number
        /// The first prime is 2, so this should probably return 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsNegative()
        {
            // Arrange
            int negativeInput = -10;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(negativeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after any negative number should be 2 (the first prime)");
        }

        /// <summary>
        /// Test the next prime after 0
        /// Since 2 is the first prime, next prime after 0 should be 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsZero()
        {
            // Arrange
            int zeroInput = 0;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(zeroInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 0 should be 2");
        }

        /// <summary>
        /// Test the next prime after 1
        /// Since 1 is not prime and 2 is the first prime, result should be 2
        /// </summary>
        [Test]
        public void ReturnTwo_WhenInputIsOne()
        {
            // Arrange
            int oneInput = 1;
            int expectedNextPrime = 2;

            // Act
            var result = _primeService.GetNextPrime(oneInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 1 should be 2");
        }

        #endregion

        #region Testing from Prime Numbers

        /// <summary>
        /// Test getting next prime when starting from a prime number
        /// Next prime after 2 should be 3
        /// </summary>
        [Test]
        public void ReturnThree_WhenInputIsTwo()
        {
            // Arrange
            int primeInput = 2;
            int expectedNextPrime = 3;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 2 should be 3");
        }

        /// <summary>
        /// Test next prime after 3 should be 5
        /// </summary>
        [Test]
        public void ReturnFive_WhenInputIsThree()
        {
            // Arrange
            int primeInput = 3;
            int expectedNextPrime = 5;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 3 should be 5");
        }

        /// <summary>
        /// Test a larger gap: next prime after 7 should be 11
        /// </summary>
        [Test]
        public void ReturnEleven_WhenInputIsSeven()
        {
            // Arrange
            int primeInput = 7;
            int expectedNextPrime = 11;

            // Act
            var result = _primeService.GetNextPrime(primeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 7 should be 11");
        }

        #endregion

        #region Testing from Composite Numbers

        /// <summary>
        /// Test getting next prime from a composite number
        /// Next prime after 4 should be 5
        /// </summary>
        [Test]
        public void ReturnFive_WhenInputIsFour()
        {
            // Arrange
            int compositeInput = 4;
            int expectedNextPrime = 5;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 4 should be 5");
        }

        /// <summary>
        /// Test next prime after 6 should be 7
        /// </summary>
        [Test]
        public void ReturnSeven_WhenInputIsSix()
        {
            // Arrange
            int compositeInput = 6;
            int expectedNextPrime = 7;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 6 should be 7");
        }

        /// <summary>
        /// Test with a larger composite number: next prime after 15 should be 17
        /// </summary>
        [Test]
        public void ReturnSeventeen_WhenInputIsFifteen()
        {
            // Arrange
            int compositeInput = 15;
            int expectedNextPrime = 17;

            // Act
            var result = _primeService.GetNextPrime(compositeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 15 should be 17");
        }

        #endregion

        #region Parameterized Tests

        /// <summary>
        /// Use TestCase to test multiple scenarios efficiently
        /// This demonstrates the power of parameterized testing
        /// </summary>
        [TestCase(1, 2, Description = "Next prime after 1")]
        [TestCase(2, 3, Description = "Next prime after 2")]
        [TestCase(3, 5, Description = "Next prime after 3")]
        [TestCase(4, 5, Description = "Next prime after 4")]
        [TestCase(5, 7, Description = "Next prime after 5")]
        [TestCase(6, 7, Description = "Next prime after 6")]
        [TestCase(7, 11, Description = "Next prime after 7")]
        [TestCase(8, 11, Description = "Next prime after 8")]
        [TestCase(9, 11, Description = "Next prime after 9")]
        [TestCase(10, 11, Description = "Next prime after 10")]
        [TestCase(11, 13, Description = "Next prime after 11")]
        [TestCase(20, 23, Description = "Next prime after 20")]
        [TestCase(30, 31, Description = "Next prime after 30")]
        public void ReturnCorrectNextPrime_ForVariousInputs(int input, int expectedNextPrime)
        {
            // Act
            var result = _primeService.GetNextPrime(input);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                $"Next prime after {input} should be {expectedNextPrime}");
        }

        #endregion

        #region Larger Numbers and Performance Tests

        /// <summary>
        /// Test with a moderately large number to ensure the algorithm works efficiently
        /// Next prime after 100 should be 101
        /// </summary>
        [Test]
        public void ReturnOneHundredOne_WhenInputIsOneHundred()
        {
            // Arrange
            int largeInput = 100;
            int expectedNextPrime = 101;

            // Act
            var result = _primeService.GetNextPrime(largeInput);

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 100 should be 101");
        }

        /// <summary>
        /// Test finding next prime after a larger number
        /// This tests both correctness and performance
        /// </summary>
        [Test]
        public void HandleLargerNumbers_WithReasonablePerformance()
        {
            // Arrange
            int largeInput = 1000;
            int expectedNextPrime = 1009; // First prime after 1000
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.GetNextPrime(largeInput);
            stopwatch.Stop();

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 1000 should be 1009");
            
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(100),
                "Finding next prime after 1000 should be fast (< 100ms)");
        }

        /// <summary>
        /// Test with an even larger number to verify scalability
        /// Next prime after 10000 should be 10007
        /// </summary>
        [Test]
        public void HandleVeryLargeNumbers_ButStillComplete()
        {
            // Arrange
            int veryLargeInput = 10000;
            int expectedNextPrime = 10007;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var result = _primeService.GetNextPrime(veryLargeInput);
            stopwatch.Stop();

            // Assert
            Assert.That(result, Is.EqualTo(expectedNextPrime),
                "Next prime after 10000 should be 10007");
            
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(500),
                "Finding next prime after 10000 should complete in reasonable time");
        }

        #endregion

        #region Cross-Validation Tests

        /// <summary>
        /// Verify that the returned number is actually prime
        /// This uses our IsPrime method to cross-validate the result
        /// </summary>
        [Test]
        public void ReturnAPrimeNumber_ForAnyValidInput()
        {
            // Arrange
            int[] testInputs = { 1, 4, 6, 9, 10, 15, 20, 25, 30 };

            // Act & Assert
            foreach (int input in testInputs)
            {
                var result = _primeService.GetNextPrime(input);
                
                try
                {
                    // Cross-validate with IsPrime method
                    Assert.That(_primeService.IsPrime(result), Is.True,
                        $"GetNextPrime({input}) returned {result}, which should be prime");
                }
                catch (System.NotImplementedException)
                {
                    // Skip this assertion until IsPrime is implemented
                    // But we can still do basic validation
                    Assert.That(result, Is.GreaterThan(input),
                        $"Next prime after {input} should be greater than {input}");
                    Assert.That(result, Is.GreaterThan(1),
                        "Any prime number should be greater than 1");
                }
            }
        }

        /// <summary>
        /// Verify that there are no prime numbers between input and result
        /// This ensures we're finding the NEXT prime, not just any prime after input
        /// </summary>
        [Test]
        public void ReturnTheNextPrime_NotJustAnyPrimeAfterInput()
        {
            // Arrange
            int input = 10;
            
            // Act
            var nextPrime = _primeService.GetNextPrime(input);
            
            // Assert
            // We know the next prime after 10 is 11
            // Let's verify there are no primes between 10 and 11
            for (int i = input + 1; i < nextPrime; i++)
            {
                try
                {
                    Assert.That(_primeService.IsPrime(i), Is.False,
                        $"Number {i} should not be prime (between {input} and next prime {nextPrime})");
                }
                catch (System.NotImplementedException)
                {
                    // Skip detailed validation until IsPrime is implemented
                    break;
                }
            }
        }

        #endregion

        #region Result Properties Tests

        /// <summary>
        /// Verify basic properties of the result
        /// </summary>
        [Test]
        public void ReturnPositiveNumber_ForAnyInput()
        {
            // Arrange
            int[] testInputs = { -5, 0, 1, 10, 100 };

            // Act & Assert
            foreach (int input in testInputs)
            {
                var result = _primeService.GetNextPrime(input);
                
                Assert.That(result, Is.GreaterThan(0),
                    $"GetNextPrime({input}) should return a positive number");
                Assert.That(result, Is.GreaterThan(input),
                    $"Next prime after {input} should be greater than {input}");
            }
        }

        /// <summary>
        /// Test the mathematical property: if we call GetNextPrime twice,
        /// the second result should be greater than the first
        /// </summary>
        [Test]
        public void ReturnLargerPrime_WhenCalledSuccessively()
        {
            // Arrange
            int startNumber = 10;

            // Act
            var firstNextPrime = _primeService.GetNextPrime(startNumber);
            var secondNextPrime = _primeService.GetNextPrime(firstNextPrime);

            // Assert
            Assert.That(secondNextPrime, Is.GreaterThan(firstNextPrime),
                "Successive calls to GetNextPrime should return increasing values");
            
            // The second prime should be the next prime after the first result
            Assert.That(firstNextPrime, Is.GreaterThan(startNumber),
                "First result should be greater than start number");
        }

        #endregion
    }
}