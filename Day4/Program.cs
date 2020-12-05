using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        private static List<Dictionary<string, string>> ReadPassports()
        {
            var passports = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>()
            };
            foreach (var line in File.ReadLines("input.txt"))
            {
                if (line.Length == 0)
                {
                    passports.Add(new Dictionary<string, string>());
                    continue;
                }

                foreach (var property in line.Split(' '))
                {
                    var kv = property.Split(':');
                    passports[^1][kv[0]] = kv[1];
                }
            }

            return passports;
        }

        private static int Part1(IEnumerable<Dictionary<string, string>> passports)
        {
            var requiredFields = new[] {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};

            return passports.Count(passport =>
                requiredFields.All(passport.ContainsKey)
            );
        }

        private static int Part2(IEnumerable<Dictionary<string, string>> passports)
        {
            var validEcl = new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
            return passports.Count(passport =>
            {
                var valid = true;
                valid &= passport.ContainsKey("byr")
                         && int.TryParse(passport["byr"], out var byr)
                         && 1920 <= byr && byr <= 2002;
                valid &= passport.ContainsKey("iyr")
                         && int.TryParse(passport["iyr"], out var iyr)
                         && 2010 <= iyr && iyr <= 2020;
                valid &= passport.ContainsKey("eyr")
                         && int.TryParse(passport["eyr"], out var eyr)
                         && 2020 <= eyr && eyr <= 2030;
                valid &= passport.ContainsKey("hgt")
                         && HgtValid(passport["hgt"]);
                valid &= passport.ContainsKey("hcl")
                         && Regex.IsMatch(passport["hcl"], "^#[0-9a-f]{6}$");
                valid &= passport.ContainsKey("ecl")
                         && validEcl.Contains(passport["ecl"]);
                valid &= passport.ContainsKey("pid")
                         && passport["pid"].Length == 9
                         && int.TryParse(passport["pid"], out var pid)
                         && pid >= 0;
                return valid;
            });
        }

        private static bool HgtValid(string hgt)
        {
            if (hgt.EndsWith("in"))
            {
                return int.TryParse(hgt.Substring(0, hgt.Length - 2), out var ghtIn)
                       && 59 <= ghtIn && ghtIn <= 76;
            }

            if (hgt.EndsWith("cm"))
            {
                return int.TryParse(hgt.Substring(0, hgt.Length - 2), out var ghtCm)
                       && 150 <= ghtCm && ghtCm <= 193;
            }

            return false;
        }

        public static void Main(string[] args)
        {
            var passports = ReadPassports();
            Console.WriteLine(Part1(passports));
            Console.WriteLine(Part2(passports));
        }
    }
}
