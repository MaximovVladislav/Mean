using System;

namespace Mean.Exceptions
{
    public class InvalidIntervalException : Exception
    {
        public InvalidIntervalException(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}
