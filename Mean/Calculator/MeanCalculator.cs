using System.Collections.Concurrent;
using Mean.Exceptions;

namespace Mean.Calculator
{
    public class MeanCalculator : IMeanCalculator
    {
        private readonly int[] _array;

        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, double>> _cache =
            new ConcurrentDictionary<int, ConcurrentDictionary<int, double>>();

        public MeanCalculator(int[] array)
        {
            _array = array;
        }

        public double GetMean(int start, int end)
        {
            if (start < 0 || start >= _array.Length || end <= start || end >= _array.Length)
            {
                throw new InvalidIntervalException(start, end);
            }

            if (_cache.TryGetValue(start, out var endMean) && endMean.TryGetValue(end, out double mean))
            {
                return mean;
            }

            double temp = 0;

            for (int i = start; i <= end; i++)
            {
                temp += _array[i];
            }

            mean = temp / (end - start + 1);
            CacheMean(mean, start, end);

            return mean;
        }

        private void CacheMean(double mean, int start, int end)
        {
            _cache.AddOrUpdate(start, key =>
            {
                var dict = new ConcurrentDictionary<int, double>();
                dict[end] = mean;
                return dict;
            }, (key, endDict) =>
            {
                endDict[end] = mean;
                return endDict;
            });
        }
    }
}
