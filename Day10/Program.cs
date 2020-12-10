using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        private static ICollection<int> ReadJoltages()
        {
            return File.ReadLines("input.txt").Select(int.Parse).ToList();
        }

        private static int Part1(ICollection<int> inputJoltages)
        {
            var joltages = new List<int>(inputJoltages);
            joltages.Sort();
            int diffOne, diffThree = diffOne = 1;
            for (var i = 0; i < joltages.Count - 1; i++)
            {
                switch (joltages[i + 1] - joltages[i])
                {
                    case 1:
                        diffOne++;
                        break;
                    case 3:
                        diffThree++;
                        break;
                }
            }

            return diffOne * diffThree;
        }

        private static ICollection<int> TopologicalSort(
            Dictionary<int, (HashSet<int> Parents, HashSet<int> Children)> graph
        )
        {
            var topoSorted = new List<int>();
            var queue = new Queue<int>();
            queue.Enqueue(graph.Keys.Max());
            while (queue.Count != 0)
            {
                var n = queue.Dequeue();
                topoSorted.Add(n);
                graph[n].Children
                    .Where(m =>
                        graph[m].Parents.All(topoSorted.Contains)
                    )
                    .ToList()
                    .ForEach(queue.Enqueue);
            }

            return topoSorted;
        }

        private static long Part2(ICollection<int> inputJoltages)
        {
            var joltages = inputJoltages.ToHashSet();
            joltages.Add(0);
            var graph = joltages.ToDictionary(
                joltage => joltage,
                joltage => (
                    Parents: Enumerable.Range(1, 3)
                        .Select(increment => joltage + increment)
                        .Where(newJoltage => joltages.Contains(newJoltage))
                        .ToHashSet(),
                    Children:
                    Enumerable.Range(1, 3)
                        .Select(decrement => joltage - decrement)
                        .Where(newJoltage => joltages.Contains(newJoltage))
                        .ToHashSet()
                )
            );
            var orderedVertices = TopologicalSort(graph).ToList();
            var counts = new Dictionary<int, long>
            {
                {orderedVertices[0], 1}
            };
            foreach (var v in orderedVertices.Skip(1))
            {
                counts.Add(v, graph[v].Parents.Select(u => counts[u]).Sum());
            }

            return counts[0];
        }

        public static void Main(string[] args)
        {
            var joltages = ReadJoltages();
            Console.WriteLine(Part1(joltages));
            Console.WriteLine(Part2(joltages));
        }
    }
}
