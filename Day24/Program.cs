using System;
using System.Collections.Generic;
using System.IO;

namespace Day24
{
    class Program
    {
        private static Floor ReadTiles()
        {
            var tiles = new HashSet<(int, int)>();
            foreach (var line in File.ReadLines("input.txt"))
            {
                var x = 0;
                var y = 0;
                for (var i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'e':
                            x--;
                            break;
                        case 'w':
                            x++;
                            break;
                        case 'n':
                            y++;
                            if (line[++i] == 'e')
                            {
                                x--;
                            }
                            break;
                        case 's':
                            y--;
                            if (line[++i] == 'w')
                            {
                                x++;
                            }
                            break;
                    }
                }

                if (tiles.Contains((x, y)))
                {
                    tiles.Remove((x, y));
                }
                else
                {
                    tiles.Add((x, y));
                }
            }

            return new Floor(tiles);
        }

        private static int Part1(Floor floor)
        {
            return floor.BlackTiles.Count;
        }

        private static int Part2(Floor floor)
        {
            for (var i = 0; i < 100; i++)
            {
                floor.Step();
            }

            return floor.BlackTiles.Count;
        }

        public static void Main(string[] args)
        {
            var floor = ReadTiles();
            Console.WriteLine(Part1(floor));
            Console.WriteLine(Part2(floor));
        }
    }
}
