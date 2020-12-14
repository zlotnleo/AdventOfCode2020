using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day14
{
    class Program
    {
        private static ICollection<IInstruction> ReadProgram()
        {
            var setMaskRegex = new Regex(
                "^mask = ([X10]+)$",
                RegexOptions.Compiled | RegexOptions.Singleline
            );

            var setValueRegex = new Regex(
                "^mem\\[(\\d+)\\] = (\\d+)$",
                RegexOptions.Compiled | RegexOptions.Singleline
            );

            return File.ReadLines("input.txt").Select(line =>
            {
                var setMaskMatch = setMaskRegex.Match(line);
                if (setMaskMatch.Success)
                {
                    return new SetMask
                    {
                        Mask = setMaskMatch.Groups[1].Value
                    } as IInstruction;
                }

                var setValueMatch = setValueRegex.Match(line);
                if (setValueMatch.Success)
                {
                    return new SetValue
                    {
                        Address = long.Parse(setValueMatch.Groups[1].Value),
                        Value = long.Parse(setValueMatch.Groups[2].Value)
                    };
                }

                return null;
            }).ToList();
        }

        private static long Part1(IEnumerable<IInstruction> instructions)
        {
            var memory = new Dictionary<long, long>();
            long andMask = 0;
            long orMask = 0;

            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case SetMask i:
                        andMask = 0;
                        orMask = 0;
                        foreach (var c in i.Mask)
                        {
                            andMask <<= 1;
                            orMask <<= 1;
                            switch (c)
                            {
                                case '1':
                                    orMask |= 1;
                                    andMask |= 1;
                                    break;
                                case 'X':
                                    andMask |= 1;
                                    break;
                            }
                        }
                        break;
                    case SetValue i:
                        memory[i.Address] = i.Value & andMask | orMask;
                        break;
                }
            }

            return memory.Values.Sum();
        }

        private static long Part2(IEnumerable<IInstruction> instructions)
        {
            static ICollection<long> GetAddresses(string mask, long address)
            {
                if (mask == "")
                {
                    return new[] {0L};
                }

                var addresses = GetAddresses(mask.Substring(0, mask.Length - 1), address >> 1);
                return mask[^1] switch
                {
                    '0' => addresses.Select(a => a << 1 | address & 1).ToList(),
                    '1' => addresses.Select(a => a << 1 | 1).ToList(),
                    'X' => addresses.Select(a => a << 1 | 1)
                        .Union(addresses.Select(a => a << 1)) // removes duplicates
                        .ToList(),
                    _ => addresses,
                };
            }

            var memory = new Dictionary<long, long>();
            var curMask = "";
            foreach (var instruction in instructions)
            {
                switch (instruction)
                {
                    case SetMask i:
                        curMask = i.Mask;
                        break;
                    case SetValue i:
                        foreach (var address in GetAddresses(curMask, i.Address))
                        {
                            memory[address] = i.Value;
                        }
                        break;
                }
            }

            return memory.Values.Sum();
        }

        public static void Main(string[] args)
        {
            var instructions = ReadProgram();
            Console.WriteLine(Part1(instructions));
            Console.WriteLine(Part2(instructions));
        }
    }
}
