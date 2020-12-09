using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        private static List<long> ReadNumbers()
        {
            return File.ReadLines("input.txt").Select(long.Parse).ToList();
        }

        private static long Part1(ICollection<long> numbers)
        {
            const int windowSize = 25;
            var window = new Queue<long>(numbers.Take(windowSize));

            foreach (var number in numbers.Skip(windowSize))
            {
                var canBeMade = window.Any(x => window.Any(y => x + y == number && x != y));
                if (!canBeMade)
                {
                    return number;
                }

                window.Dequeue();
                window.Enqueue(number);
            }

            return long.MinValue;
        }

        private static long Part2(IList<long> numbers, long target)
        {
            for (var offset = 0; offset < numbers.Count - 2; offset++)
            {
                long sum, min, max = min = sum = numbers[offset];
                for (var subsetSize = 2; subsetSize <= numbers.Count - offset; subsetSize++)
                {
                    var nextNum = numbers[offset + subsetSize - 1];
                    sum += nextNum;
                    min = Math.Min(min, nextNum);
                    max = Math.Max(max, nextNum);

                    if (sum == target)
                    {
                        return min + max;
                    }
                }
            }

            return long.MinValue;
        }

        public static void Main(string[] args)
        {
            var numbers = ReadNumbers();
            var notSumOfTwoInWindow = Part1(numbers);
            Console.WriteLine(notSumOfTwoInWindow);
            Console.WriteLine(Part2(numbers, notSumOfTwoInWindow));
        }
    }
}
