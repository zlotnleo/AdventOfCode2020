using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        private static (IDictionary<int, string> rules, ICollection<string> messages) ReadRulesAndMessages()
        {
            var readRules = true;
            var rules = new Dictionary<int, string>();
            var messages = new List<string>();
            var ruleRegex = new Regex("^(\\d+): (.*)$", RegexOptions.Singleline | RegexOptions.Compiled);

            foreach (var line in File.ReadLines("input.txt"))
            {
                if (line.Length == 0)
                {
                    readRules = false;
                    continue;
                }

                if (readRules)
                {
                    var match = ruleRegex.Match(line);
                    rules[int.Parse(match.Groups[1].Value)] = match.Groups[2].Value;
                }
                else
                {
                    messages.Add(line);
                }
            }

            return (rules, messages);
        }

        private static int Part1(IDictionary<int, string> rules, ICollection<string> messages)
        {
            var regexCache = new Dictionary<int, string>();

            string GetRulePattern(int index)
            {
                if (!regexCache.TryGetValue(index, out var pattern))
                {
                    var rule = rules[index];
                    pattern = rule.StartsWith('"')
                        ? rule.Substring(1, rule.Length - 2)
                        : string.Join("|",
                            rules[index].Split('|')
                                .Select(term => string.Concat(
                                        term.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(ruleStr => $"(?:{GetRulePattern(int.Parse(ruleStr))})")
                                    )
                                )
                        );

                    regexCache[index] = pattern;
                }

                return pattern;
            }

            var regex = new Regex($"^{GetRulePattern(0)}$", RegexOptions.Compiled | RegexOptions.Singleline);
            return messages.Count(m => regex.Match(m).Success);
        }

        private static int Part2(IDictionary<int, string> rules, ICollection<string> messages)
        {
            var regexCache = new Dictionary<int, string>();

            string GetRulePattern(int index)
            {
                if (!regexCache.TryGetValue(index, out var pattern))
                {
                    var rule = rules[index];
                    pattern = index switch
                    {
                        8 => $"(?:{GetRulePattern(42)})+",
                        11 => $"(?<Stack>{GetRulePattern(42)})+(?<-Stack>{GetRulePattern(31)})+(?(Stack)(?!))",
                        _ => rule.StartsWith('"')
                            ? rule.Substring(1, rule.Length - 2)
                            : string.Join("|",
                                rules[index]
                                    .Split('|')
                                    .Select(term => string.Concat(
                                        term.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(ruleStr => $"(?:{GetRulePattern(int.Parse(ruleStr))})"))))
                    };

                    regexCache[index] = pattern;
                }

                return pattern;
            }

            var regex = new Regex($"^{GetRulePattern(0)}$", RegexOptions.Compiled | RegexOptions.Singleline);
            return messages.Count(m => regex.Match(m).Success);
        }

        public static void Main(string[] args)
        {
            var (rules, messages) = ReadRulesAndMessages();
            Console.WriteLine(Part1(rules, messages));
            Console.WriteLine(Part2(rules, messages));
        }
    }
}
