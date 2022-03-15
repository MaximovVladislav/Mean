using System.Collections.Generic;
using System.Threading.Tasks;
using Mean.Calculator;
using Mean.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mean.Tests
{
    [TestClass]
    public class MeanCalculatorTests
    {
        // [4, 2, 3, 1, 5]
        private static Dictionary<(int, int), double> _predefinedMeans = new Dictionary<(int, int), double>()
        {
             { (0, 1), 3 },
             { (0, 2), 3 },
             { (0, 3), 2.5 },
             { (0, 4), 3 },
             { (1, 2), 2.5 },
             { (1, 3), 2 },
             { (1, 4), 2.75 },
             { (2, 3), 2 },
             { (2, 4), 3 },
             { (3, 4), 3 }
        };

        [TestMethod]
        public void GetMeanTest()
        {
            int[] array = { 4, 2, 3, 1, 5 };

            MeanCalculator calculator = new MeanCalculator(array);
            AssertMeans(calculator, array.Length);
        }

        [TestMethod]
        public void GetMeanTest_Parallel()
        {
            int[] array = { 4, 2, 3, 1, 5 };

            MeanCalculator calculator = new MeanCalculator(array);
            // iterate all the means 10 times in parallel
            Parallel.For(0, 10, i => AssertMeans(calculator, array.Length));
        }

        [TestMethod]
        public void GetMeanTest_Zeros()
        {
            int[] array = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            MeanCalculator calculator = new MeanCalculator(array);
            Assert.AreEqual(0, calculator.GetMean(1, 6));
        }

        [TestMethod]
        public void GetMeanTest_Exception()
        {
            int[] array = { 4, 2, 3, 1, 5 };

            MeanCalculator calculator = new MeanCalculator(array);

            Assert.ThrowsException<InvalidIntervalException>(() => calculator.GetMean(2, 1));
            Assert.ThrowsException<InvalidIntervalException>(() => calculator.GetMean(-1, 1));
            Assert.ThrowsException<InvalidIntervalException>(() => calculator.GetMean(1, 5));
        }

        private static void AssertMeans(MeanCalculator meanCalculator, int length)
        {
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    var mean = meanCalculator.GetMean(i, j);
                    Assert.AreEqual(_predefinedMeans[(i, j)], mean);
                }
            }
        }
    }
}
