using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        private static IList<IList<IList<char>>> GetGroups()
        {
            var groups = new List<IList<IList<char>>>
            {
                new List<IList<char>>()
            };
            foreach (var line in File.ReadLines("input.txt"))
            {
                if (line.Length == 0)
                {
                    groups.Add(new List<IList<char>>());
                    continue;
                }
                groups[^1].Add(new List<char>(line));
            }

            return groups;
        }

        private static int Part1(IList<IList<IList<char>>> groups)
        {
            return groups
                .Select(group => group.Aggregate((p1, p2) => p1.Union(p2).ToList()).Count)
                .Aggregate((x, y) => x + y);
        }

        private static int Part2(IList<IList<IList<char>>> groups)
        {
            return groups
                .Select(group => group.Aggregate((p1, p2) => p1.Intersect(p2).ToList()).Count)
                .Aggregate((x, y) => x + y);
        }

        public static void Main(string[] args)
        {
            var groups = GetGroups();
            Console.WriteLine(Part1(groups));
            Console.WriteLine(Part2(groups));
        }
    }
}
