using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Program
    {
        private static ICollection<(List<string>, List<string>)> ReadFoodList()
        {
            var regex = new Regex("^(?:(\\w+) )+\\(contains (?:(\\w+)(?:, )?)+\\)",
                RegexOptions.Compiled | RegexOptions.Singleline);
            return File.ReadLines("input.txt")
                .Select(line => regex.Match(line))
                .Select(match => (
                    match.Groups[1].Captures.Select(c => c.Value).ToList(),
                    match.Groups[2].Captures.Select(c => c.Value).ToList()
                ))
                .ToList();
        }

        private static int Part1(ICollection<(List<string> ingredients, List<string> allergens)> food)
        {
            var ingredientsWithPotentialAllergens = food.SelectMany(item => item.allergens)
                .Distinct()
                .SelectMany(allergen =>
                    food.Where(item => item.allergens!.Contains(allergen))
                        .Select(item => item.ingredients)
                        .Aggregate<IEnumerable<string>>((ing1, ing2) => ing1.Intersect(ing2!))
                )
                .ToHashSet();

            return food
                .SelectMany(item => item.ingredients)
                .Count(ingredient => !ingredientsWithPotentialAllergens.Contains(ingredient));
        }

        private static string Part2(ICollection<(List<string> ingredients, List<string> allergens)> food)
        {
            var allergenCandidates = food.SelectMany(item => item.allergens)
                .Distinct()
                .ToDictionary(
                    allergen => allergen,
                    allergen => food.Where(item => item.allergens!.Contains(allergen))
                        .Select(item => item.ingredients)
                        .Aggregate<IEnumerable<string>>((ing1, ing2) => ing1.Intersect(ing2!))
                        .ToList()
                );
            var definitiveAllergens = new List<(string allergen, string ingredient)>();

            while (allergenCandidates.Count != 0)
            {
                var (allergen, value) = allergenCandidates.First(c => c.Value.Count == 1);
                var ingredient = value.Single();
                definitiveAllergens.Add((allergen, ingredient));
                allergenCandidates.Remove(allergen);
                foreach (var otherAllergen in allergenCandidates.Keys)
                {
                    allergenCandidates[otherAllergen].Remove(ingredient);
                }
            }

            return string.Join(",", definitiveAllergens.OrderBy(t => t.allergen).Select(t => t.ingredient));
        }

        static void Main(string[] args)
        {
            var food = ReadFoodList();
            Console.WriteLine(Part1(food));
            Console.WriteLine(Part2(food));
        }
    }
}
