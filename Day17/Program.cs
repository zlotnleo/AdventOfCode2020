using System;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        private static bool[][] GetInitialSlice()
        {
            return File.ReadLines("input.txt")
                .Select(line => line.Select(c => c == '#').ToArray())
                .ToArray();
        }

        private static int CountAfter6Cycles(int dimensions, bool[][] initialSlice)
        {
            var pocketDimension = new PocketDimension(dimensions, initialSlice);
            for (var i = 0; i < 6; i++)
            {
                pocketDimension.Step();
            }

            return pocketDimension.CountActive();
        }

        public static void Main(string[] args)
        {
            var initialSlice = GetInitialSlice();
            Console.WriteLine(CountAfter6Cycles(3, initialSlice));
            Console.WriteLine(CountAfter6Cycles(4, initialSlice));
        }
    }
}
