using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        public class Node
        {
            public int Value;
            public Node Next;
            public Node Prev;
        }

        private static ICollection<int> ReadCups()
        {
            return File.ReadLines("input.txt").First().Select(c => c - '0').ToList();
        }

        private static IEnumerable<int> CrabGame(ICollection<int> initialCups, int numMoves)
        {
            var firstCup = initialCups.First();
            var nodeLookup = new Dictionary<int, Node>();
            var currentCup = new Node
            {
                Value = firstCup
            };
            nodeLookup.Add(firstCup, currentCup);
            var minCup = firstCup;
            var maxCup = firstCup;
            var lastNode = currentCup;
            foreach (var initialCup in initialCups.Skip(1))
            {
                var node = new Node
                {
                    Value = initialCup,
                    Prev = lastNode
                };
                nodeLookup.Add(initialCup, node);
                lastNode.Next = node;
                lastNode = node;

                if (initialCup < minCup)
                {
                    minCup = initialCup;
                }

                if (initialCup > maxCup)
                {
                    maxCup = initialCup;
                }
            }

            currentCup.Prev = lastNode;
            lastNode.Next = currentCup;

            for (var i = 0; i < numMoves; i++)
            {
                var destination = currentCup.Value;
                Node destinationNode;
                do
                {
                    destination--;
                    if (destination < minCup)
                    {
                        destination = maxCup;
                    }
                } while (!nodeLookup.TryGetValue(destination, out destinationNode)
                         || destinationNode.Prev == currentCup
                         || destinationNode.Prev.Prev == currentCup
                         || destinationNode.Prev.Prev.Prev == currentCup);

                var startOfSlice = currentCup.Next;
                var endOfSlice = startOfSlice.Next.Next;
                endOfSlice.Next.Prev = currentCup;
                startOfSlice.Prev = destinationNode;
                destinationNode.Next.Prev = endOfSlice;
                currentCup.Next = endOfSlice.Next;
                endOfSlice.Next = destinationNode.Next;
                destinationNode.Next = startOfSlice;

                currentCup = currentCup.Next;
            }

            for (var node = nodeLookup[1].Next; node.Value != 1; node = node.Next)
            {
                yield return node.Value;
            }
        }

        private static int Part1(ICollection<int> cups)
        {
            return CrabGame(cups, 100)
                .Aggregate(0, (acc, elt) => acc * 10 + elt);
        }

        private static long Part2(ICollection<int> cups)
        {
            var updatedCups = cups.Concat(Enumerable.Range(cups.Max() + 1, 1000000 - cups.Count)).ToList();
            var cupsWithStars = CrabGame(updatedCups, 10000000).Take(2).ToList();
            return (long) cupsWithStars[0] * cupsWithStars[1];
        }

        public static void Main(string[] args)
        {
            var cups = ReadCups();
            Console.WriteLine(Part1(cups));
            Console.WriteLine(Part2(cups));
        }
    }
}
