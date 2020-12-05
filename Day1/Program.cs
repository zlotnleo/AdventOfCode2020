using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        private static (int, int)? NumbersAddingUpToTotal(ISet<int> numbers, int total)
        {
            var firstNumber = numbers.Cast<int?>()
                .FirstOrDefault(n => n.HasValue && numbers.Contains(total - n.Value));
            if (firstNumber.HasValue)
            {
                return (firstNumber.Value, total - firstNumber.Value);
            }

            return null;
        }

        private static int Part1(ISet<int> numbers)
        {
            var (n1, n2) = NumbersAddingUpToTotal(numbers, 2020) ?? default;
            return n1 * n2;
        }

        private static int Part2(ISet<int> numbers)
        {
            foreach (var n1 in numbers)
            {
                var other = NumbersAddingUpToTotal(numbers, 2020 - n1);
                if (other.HasValue)
                {
                    var (n2, n3) = other.Value;
                    return n1 * n2 * n3;
                }
            }

            return -1;
        }

        public static void Main()
        {
            var numbers = File.ReadLines("input.txt").Select(int.Parse).ToHashSet();
            Console.WriteLine(Part1(numbers));
            Console.WriteLine(Part2(numbers));
        }
    }
}
