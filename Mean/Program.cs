using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mean.Calculator;

namespace Mean
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 4, 2, 3, 1, 5 };

            Console.WriteLine($"[{string.Join(", ", array)}]");

            var meanCalc1 = new MeanCalculator(array);
            IterateMeans(meanCalc1, array.Length);

            var meanCalc2 = new CachedMeanCalculator(array);
            IterateMeans(meanCalc2, array.Length);

            var meanCalc3 = new HybridCachedMeanCalculator(array);
            IterateMeans(meanCalc3, array.Length);

            // too slow
            //PerformanceTest(a => new MeanCalculator(a));
            Console.WriteLine("\nCached:\n");
            PerformanceTest(a => new CachedMeanCalculator(a));
            Console.WriteLine("\nHybryd:\n");
            PerformanceTest(a => new HybridCachedMeanCalculator(a));
        }

        static void PerformanceTest(Func<int[], IMeanCalculator> createMeanCalculator)
        {
            int[] array = new int[10000];

            Random rand = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next();
            }

            DateTime startTime = DateTime.Now;

            var meanCalc = createMeanCalculator(array);

            Console.WriteLine($"Initialization duration: {DateTime.Now - startTime}");

            // iterate all the means three times in parallell
            Parallel.For(0, 3, i => IterateMeans(meanCalc, array.Length, false));            

            Console.WriteLine($"Duration (All): {DateTime.Now - startTime}\n");
        }

        static void IterateMeans(IMeanCalculator meanCalculator, int length, bool logResult = true)
        {
            if (logResult)
            {
                Console.WriteLine($"\n{meanCalculator.GetType().Name}:\n");
            }

            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    var mean = meanCalculator.GetMean(i, j);
                    if (logResult)
                    {
                        Console.WriteLine($"({i}, {j}): {mean:0.####}");
                    }
                }
            }

            if (logResult)
            {
                Console.WriteLine();
            }
        }
    }
}
