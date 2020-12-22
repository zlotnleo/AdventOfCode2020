using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        private class DecksEqualityComparer : IEqualityComparer<(IReadOnlyCollection<int>, IReadOnlyCollection<int>)>
        {
            public bool Equals((IReadOnlyCollection<int>, IReadOnlyCollection<int>) x, (IReadOnlyCollection<int>, IReadOnlyCollection<int>) y)
            {
                return x.Item1!.SequenceEqual(y.Item1!) && x.Item2!.SequenceEqual(y.Item2!);
            }

            public int GetHashCode((IReadOnlyCollection<int>, IReadOnlyCollection<int>) obj)
            {
                const int prime = 31;
                return obj.Item1!.Concat(obj.Item2!).Aggregate(0, (hash, element) => hash + prime * element);
            }
        }

        private static readonly DecksEqualityComparer DecksComparer = new DecksEqualityComparer();

        private static (ICollection<int>, ICollection<int>) ReadDecks()
        {
            var players = new[]
            {
                new List<int>(),
                new List<int>()
            };
            var player = 0;
            foreach (var line in File.ReadLines("input.txt"))
            {
                if (line == "")
                {
                    player = 1;
                    continue;
                }

                if (line.StartsWith("Player"))
                {
                    continue;
                }

                players[player].Add(int.Parse(line));
            }

            return (players[0], players[1]);
        }

        private static int Part1(ICollection<int> deck1, ICollection<int> deck2)
        {
            var p1 = new Queue<int>(deck1);
            var p2 = new Queue<int>(deck2);

            while (p1.Any() && p2.Any())
            {
                var card1 = p1.Dequeue();
                var card2 = p2.Dequeue();
                if (card1 > card2)
                {
                    p1.Enqueue(card1);
                    p1.Enqueue(card2);
                }
                else
                {
                    p2.Enqueue(card2);
                    p2.Enqueue(card1);
                }
            }

            return (p1.Any() ? p1 : p2).Reverse().Select((card, i) => card * (i + 1)).Sum();
        }

        private static (bool p1Won, IReadOnlyCollection<int> winDeck) RecursiveGame(
            IEnumerable<int> deck1, IEnumerable<int> deck2)
        {
            var previousRounds = new HashSet<(IReadOnlyCollection<int>, IReadOnlyCollection<int>)>(DecksComparer);
            var p1 = new Queue<int>(deck1);
            var p2 = new Queue<int>(deck2);

            while (p1.Any() && p2.Any())
            {
                if (previousRounds.Contains((p1, p2)))
                {
                    return (true, p1);
                }

                previousRounds.Add((new List<int>(p1), new List<int>(p2)));
                var card1 = p1.Dequeue();
                var card2 = p2.Dequeue();

                bool p1Won;
                if (p1.Count >= card1 && p2.Count >= card2)
                {
                    p1Won = RecursiveGame(p1.Take(card1), p2.Take(card2)).p1Won;
                }
                else
                {
                    p1Won = card1 > card2;
                }

                if (p1Won)
                {
                    p1.Enqueue(card1);
                    p1.Enqueue(card2);
                }
                else
                {
                    p2.Enqueue(card2);
                    p2.Enqueue(card1);
                }
            }

            return p1.Any() ? (true, p1) : (false, p2);
        }

        private static int Part2(ICollection<int> deck1, ICollection<int> deck2)
        {
            return RecursiveGame(deck1, deck2).winDeck.Reverse().Select((card, i) => card * (i + 1)).Sum();
        }

        public static void Main(string[] args)
        {
            var (deck1, deck2) = ReadDecks();
            Console.WriteLine(Part1(deck1, deck2));
            Console.WriteLine(Part2(deck1, deck2));
        }
    }
}
