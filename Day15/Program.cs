using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        private static IList<int> ReadInitialNumbers()
        {
            return File.ReadLines("input.txt")
                .First()
                .Split(',')
                .Select(int.Parse)
                .ToList();
        }

        private static int Parts12(IList<int> initialNumbers, int lastPosition)
        {
            int i;
            var lastSpokenTurnLookup = new Dictionary<int, int>();
            for (i = 0; i < initialNumbers.Count; i++)
            {
                lastSpokenTurnLookup[initialNumbers[i]] = i;
            }

            var lastSpoken = initialNumbers[^1];
            for (; i < lastPosition; i++)
            {
                var t = lastSpokenTurnLookup.GetValueOrDefault(lastSpoken, i - 1);
                lastSpokenTurnLookup[lastSpoken] = i - 1;
                lastSpoken = i - 1 - t;
            }

            return lastSpoken;
        }

        public static void Main(string[] args)
        {
            var initialNumbers = ReadInitialNumbers();
            Console.WriteLine(Parts12(initialNumbers, 2020));
            Console.WriteLine(Parts12(initialNumbers, 30000000));
        }
    }
}
