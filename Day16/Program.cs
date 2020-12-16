using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        private static (Dictionary<string, ((int, int), (int, int))> rules, List<int> myTicket, List<List<int>>
            otherTickets) Read()
        {
            const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.Singleline;
            using var lineEnumerator = File.ReadLines("input.txt").GetEnumerator();
            string line;
            Match match;

            var rulesRegex = new Regex("([\\w\\s]*): (\\d+)-(\\d+) or (\\d+)-(\\d+)", regexOptions);
            var rules = new Dictionary<string, ((int, int), (int, int))>();

            while (lineEnumerator.MoveNext()
                   && (line = lineEnumerator.Current) != null
                   && line.Length != 0)
            {
                match = rulesRegex.Match(line);
                rules[match.Groups[1].Value] = (
                    (int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)),
                    (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value))
                );
            }

            var ticketRegex = new Regex("(?:(\\d+),?)+");
            List<int> myTicket = new List<int>();

            while (lineEnumerator.MoveNext()
                   && (line = lineEnumerator.Current) != null
                   && line.Length != 0)
            {
                match = ticketRegex.Match(line);
                if (match.Success)
                {
                    myTicket = match.Groups[1].Captures.Select(c => int.Parse(c.Value)).ToList();
                }
            }

            List<List<int>> otherTickets = new List<List<int>>();

            while (lineEnumerator.MoveNext()
                   && (line = lineEnumerator.Current) != null
                   && line.Length != 0)
            {
                match = ticketRegex.Match(line);
                if (match.Success)
                {
                    otherTickets.Add(match.Groups[1].Captures.Select(c => int.Parse(c.Value)).ToList());
                }
            }

            return (rules, myTicket, otherTickets);
        }

        private static bool MatchesRule(int value, ((int, int), (int, int)) rule)
        {
            var ((min1, max1), (min2, max2)) = rule;
            return value >= min1 && value <= max1
                   || value >= min2 && value <= max2;
        }

        private static int Part1(Dictionary<string, ((int, int), (int, int))> rules,
            ICollection<List<int>> otherTickets)
        {
            return otherTickets.SelectMany(ticket =>
                ticket.Where(value =>
                    rules.All(rule =>
                        !MatchesRule(value, rule.Value)
                    )
                )
            ).Sum();
        }

        private static long Part2(Dictionary<string, ((int, int), (int, int))> rules,
            List<int> myTicket,
            ICollection<List<int>> otherTickets)
        {
            var allValidTickets = otherTickets.Where(ticket =>
                    ticket.All(value =>
                        rules.Any(rule =>
                            MatchesRule(value, rule.Value)
                        )
                    )
                )
                .Concat(new[] {myTicket.ToList()})
                .ToList();

            var matchingRules = new List<(ICollection<string>, int)>();
            for (var i = 0; i < myTicket.Count; i++)
            {
                var fieldValues = allValidTickets.Select(ticket => ticket[i]).ToList();
                var validRules = rules.Where(rule =>
                        fieldValues.All(value =>
                            MatchesRule(value, rule.Value)
                        )
                    )
                    .Select(rule => rule.Key)
                    .ToList();
                matchingRules.Add((validRules, i));
            }

            var ruleToIndexMapping = new Dictionary<string, int>();
            foreach (var (validRules, index) in matchingRules.OrderBy(validRules => validRules.Item1?.Count))
            {
                var rule = validRules.Except(ruleToIndexMapping.Keys.AsEnumerable()).Single();
                ruleToIndexMapping[rule] = index;
            }

            return ruleToIndexMapping.Where(entry => entry.Key.StartsWith("departure"))
                .Select(entry => (long) myTicket[entry.Value])
                .Aggregate((x, y) => x * y);
        }

        public static void Main(string[] args)
        {
            var (rules, myTicket, otherTickets) = Read();
            Console.WriteLine(Part1(rules, otherTickets));
            Console.WriteLine(Part2(rules, myTicket, otherTickets));
        }
    }
}
