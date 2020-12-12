using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        private static ICollection<(char, int)> ReadActions() =>
            File.ReadLines("input.txt")
                .Select(l => (l[0], int.Parse(l.Substring(1))))
                .ToList();

        private static int Part1(IEnumerable<(char, int)> actions)
        {
            var position = (x: 0, y: 0);
            var currentDir = 'E';
            foreach (var (tmpAction, value) in actions)
            {
                var action = tmpAction;
                if (action == 'F')
                {
                    action = currentDir;
                }

                switch (action, currentDir, value)
                {
                    case ('R', 'S', 90):
                    case ('L', 'S', 270):
                    case ('R', 'E', 180):
                    case ('L', 'E', 180):
                    case ('R', 'N', 270):
                    case ('L', 'N', 90):
                        currentDir = 'W';
                        break;
                    case ('R', 'E', 90):
                    case ('L', 'E', 270):
                    case ('R', 'N', 180):
                    case ('L', 'N', 180):
                    case ('R', 'W', 270):
                    case ('L', 'W', 90):
                        currentDir = 'S';
                        break;
                    case ('R', 'N', 90):
                    case ('L', 'N', 270):
                    case ('R', 'W', 180):
                    case ('L', 'W', 180):
                    case ('R', 'S', 270):
                    case ('L', 'S', 90):
                        currentDir = 'E';
                        break;
                    case ('R', 'W', 90):
                    case ('L', 'W', 270):
                    case ('R', 'S', 180):
                    case ('L', 'S', 180):
                    case ('R', 'E', 270):
                    case ('L', 'E', 90):
                        currentDir = 'N';
                        break;
                    case ('E', _, _):
                        position.x += value;
                        break;
                    case ('N', _, _):
                        position.y += value;
                        break;
                    case ('W', _, _):
                        position.x -= value;
                        break;
                    case ('S', _, _):
                        position.y -= value;
                        break;
                }
            }

            return Math.Abs(position.x) + Math.Abs(position.y);
        }

        private static int Part2(IEnumerable<(char, int)> actions)
        {
            var position = (x: 0, y: 0);
            var waypoint = (x: 10, y: 1);

            foreach (var (action, value) in actions)
            {
                if (action == 'F')
                {
                    position.x += waypoint.x * value;
                    position.y += waypoint.y * value;
                    continue;
                }

                switch (action, value)
                {
                    case ('E', _):
                        waypoint.x += value;
                        break;
                    case ('N', _):
                        waypoint.y += value;
                        break;
                    case ('W', _):
                        waypoint.x -= value;
                        break;
                    case ('S', _):
                        waypoint.y -= value;
                        break;
                    case ('R', 90):
                    case ('L', 270):
                        waypoint = (waypoint.y, -waypoint.x);
                        break;
                    case ('R', 180):
                    case ('L', 180):
                        waypoint = (-waypoint.x, -waypoint.y);
                        break;
                    case ('R', 270):
                    case ('L', 90):
                        waypoint = (-waypoint.y, waypoint.x);
                        break;
                }
            }

            return Math.Abs(position.x) + Math.Abs(position.y);
        }

        public static void Main(string[] args)
        {
            var actions = ReadActions();
            Console.WriteLine(Part1(actions));
            Console.WriteLine(Part2(actions));
        }
    }
}
