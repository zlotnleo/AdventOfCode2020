using System;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        private static bool[][] GetTreeMap()
        {
            return File.ReadLines("input.txt")
                .Select(line =>
                    line.ToCharArray().Select(c => c == '#').ToArray()
                )
                .ToArray();
        }

        private static int CountTrees(bool[][] treeMap, int dx, int dy)
        {
            var count = 0;

            for (int y = 0, x = 0; y < treeMap.Length; x = (x + dx) % treeMap[y].Length, y += dy)
            {
                if (treeMap[y][x])
                {
                    count++;
                }
            }

            return count;
        }

        private static int Part1(bool[][] treeMap)
        {
            return CountTrees(treeMap, 3, 1);
        }

        private static long Part2(bool[][] treeMap)
        {
            return new (int x, int y)[] {(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)}
                .Select(d => (long) CountTrees(treeMap, d.x, d.y))
                .Aggregate((m, n) => m * n);
        }

        public static void Main()
        {
            var treeMap = GetTreeMap();
            Console.WriteLine(Part1(treeMap));
            Console.WriteLine(Part2(treeMap));
        }
    }
}
