using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day7
{
    class Program
    {
        private static Dictionary<string, Dictionary<string, int>> ReadRules()
        {
            var rules = new Dictionary<string, Dictionary<string, int>>();

            var regex = new Regex(
                "^([a-z]+ [a-z]+) bags contain(?:(?: (\\d+) ([a-z]+ [a-z]+) bags?[,\\.])+| no other bags\\.)$",
                RegexOptions.Singleline | RegexOptions.Compiled
            );
            foreach (var line in File.ReadLines("input.txt"))
            {
                var match = regex.Match(line);
                var outerBag = match.Groups[1].Value;

                var innerBags = match.Groups[2].Captures.Zip(
                    match.Groups[3].Captures,
                    (countCapture, colourCapture) => (int.Parse(countCapture.Value), colourCapture.Value)
                );

                rules.Add(outerBag, new Dictionary<string, int>());
                foreach (var (count, innerBag) in innerBags)
                {
                    rules[outerBag].Add(innerBag, count);
                }
            }

            return rules;
        }

        private static int Part1(Dictionary<string, Dictionary<string, int>> rules)
        {
            var visited = new HashSet<string>();
            var queue = new Queue<string>();
            queue.Enqueue("shiny gold");

            while (queue.Count != 0)
            {
                var colour = queue.Dequeue();
                if (visited.Contains(colour))
                {
                    continue;
                }

                visited.Add(colour);

                rules.Where(r => r.Value.ContainsKey(colour))
                    .Select(r => r.Key)
                    .ToList()
                    .ForEach(c => queue.Enqueue(c));
            }

            return visited.Count - 1;
        }

        private static int Part2(Dictionary<string, Dictionary<string, int>> rules)
        {
            var savedCounts = new Dictionary<string, int>();

            int Count(string colour)
            {
                if (savedCounts.TryGetValue(colour, out var savedCount))
                {
                    return savedCount;
                }

                var count = rules[colour]
                    .Select(rule => (Count(rule.Key) + 1) * rule.Value)
                    .Aggregate(0, (x, y) => x + y);
                savedCounts[colour] = count;
                return count;
            }

            return Count("shiny gold");
        }

        public static void Main(string[] args)
        {
            var rules = ReadRules();
            Console.WriteLine(Part1(rules));
            Console.WriteLine(Part2(rules));
        }
    }
}
