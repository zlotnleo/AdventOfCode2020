using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day2
{
    class Program
    {
        private static ICollection<PasswordData> ReadData()
        {
            var regex = new Regex("^(\\d+)-(\\d+) (\\w): (\\w+)$", RegexOptions.Compiled | RegexOptions.Singleline);
            return File.ReadLines("input.txt")
                .Select(line =>
                {
                    var match = regex.Match(line);
                    return new PasswordData
                    {
                        Min = int.Parse(match.Groups[1].Value),
                        Max = int.Parse(match.Groups[2].Value),
                        Character = match.Groups[3].Value[0],
                        Password = match.Groups[4].Value
                    };
                }).ToList();
        }

        private static int Part1(IEnumerable<PasswordData> passwords)
        {
            return passwords.Count(password =>
            {
                var count = password.Password.Count(c => c == password.Character);
                return password.Min <= count && count <= password.Max;
            });
        }

        private static int Part2(IEnumerable<PasswordData> passwords)
        {
            return passwords.Count(password =>
                password.Password[password.Min - 1] == password.Character
                ^ password.Password[password.Max - 1] == password.Character
            );
        }

        public static void Main()
        {
            var passwords = ReadData();
            Console.WriteLine(Part1(passwords));
            Console.WriteLine(Part2(passwords));
        }
    }
}
