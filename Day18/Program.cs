using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        private static ICollection<ICollection<IToken>> ReadExpressions() =>
            File.ReadLines("input.txt")
                .Select(Tokeniser.Tokenise)
                .ToList();

        private static long Part1(ICollection<ICollection<IToken>> expressions)
        {
            var precedences = new Dictionary<Type, int>
            {
                {typeof(Plus), 1},
                {typeof(Times), 1}
            };
            return expressions.Select(line => Evaluator.Evaluate(line, precedences))
                .Sum();
        }

        private static long Part2(ICollection<ICollection<IToken>> expressions)
        {
            var precedences = new Dictionary<Type, int>
            {
                {typeof(Plus), 2},
                {typeof(Times), 1}
            };
            return expressions.Select(line => Evaluator.Evaluate(line, precedences))
                .Sum();
        }

        public static void Main(string[] args)
        {
            var expressions = ReadExpressions();
            Console.WriteLine(Part1(expressions));
            Console.WriteLine(Part2(expressions));
        }
    }
}
