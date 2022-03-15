using System.Collections.Concurrent;
using System.Threading.Tasks;
using Mean.Exceptions;

namespace Mean.Calculator
{
    public class CachedMeanCalculator : IMeanCalculator
    {

        private readonly ConcurrentDictionary<int, ConcurrentDictionary<int, double>> _cache =
            new ConcurrentDictionary<int, ConcurrentDictionary<int, double>>();
        private readonly int _length;

        public CachedMeanCalculator(int[] array)
        {
            _length = array.Length;
            Parallel.For(0, _length - 1, i =>
            {
                double summ = array[i];
                for (int j = i + 1; j < _length; j++)
                {
                    summ += array[j];
                    CacheMean(summ / (j - i + 1), i, j);
                }
            });
        }

        public double GetMean(int start, int end)
        {
            if (start < 0 || start >= _length || end <= start || end >= _length)
            {
                throw new InvalidIntervalException(start, end);
            }

            return _cache[start][end];
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
