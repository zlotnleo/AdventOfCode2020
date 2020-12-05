using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        private static List<int> GetSeatIds()
        {
            return File.ReadLines("input.txt")
                .Select(line => Convert.ToInt32(
                    line.Replace("F", "0").Replace("B", "1").Replace("R", "1").Replace("L", "0"),
                    2
                ))
                .ToList();
        }

        private static int Part1(IEnumerable<int> seatIds)
        {
            return seatIds.Max();
        }

        private static int Part2(IEnumerable<int> seatIds)
        {
            var idSet = seatIds.ToHashSet();
            return Enumerable.Range(0, idSet.Max())
                .First(id => !idSet.Contains(id) && idSet.Contains(id - 1) && idSet.Contains(id + 1));
        }

        static void Main(string[] args)
        {
            var seatIds = GetSeatIds();
            Console.WriteLine(Part1(seatIds));
            Console.WriteLine(Part2(seatIds));
        }
    }
}
