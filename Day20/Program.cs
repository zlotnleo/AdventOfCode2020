using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        private static ICollection<Tile> GetTiles()
        {
            var tiles = new List<Tile>();
            foreach (var line in File.ReadLines("input.txt"))
            {
                if (line.Length == 0)
                {
                    continue;
                }

                if (line.StartsWith("Tile"))
                {
                    tiles.Add(new Tile
                    {
                        Id = int.Parse(line.Substring("Tile".Length, line.Length - "Tile".Length - 1)),
                        Data = new List<List<char>>()
                    });
                    continue;
                }

                tiles[^1].Data.Add(line.ToList());
            }

            foreach (var tile in tiles)
            {
                tile.FindNeighbours(tiles);
            }

            return tiles;
        }

        private static long Part1(ICollection<Tile> tiles)
        {
            return tiles.Where(tile => tile.Neighbours.Values.Count(n => n == null) == 2)
                .Select(t => (long) t.Id)
                .Aggregate((x, y) => x * y);
        }

        private static List<List<Tile>> ArrangeTiles(ICollection<Tile> tiles)
        {
            var topLeftCorner = tiles.First(t => t.Neighbours.Values.Count(n => n == null) == 2);

            while (topLeftCorner.Neighbours[Side.Up] != null || topLeftCorner.Neighbours[Side.Left] != null)
            {
                topLeftCorner.RotateRight();
            }

            var topRow = new List<Tile> {topLeftCorner};
            Tile tile;
            while ((tile = topRow[^1].Neighbours[Side.Right]) != null)
            {
                tile.OrientLeft(topRow[^1]);
                topRow.Add(tile);
            }

            var rows = new List<List<Tile>> {topRow};
            while (rows[^1][0].Neighbours[Side.Down] != null)
            {
                var newRow = rows[^1].Select(lastRowTile =>
                {
                    var newRowTile = lastRowTile.Neighbours[Side.Down];
                    newRowTile.OrientUp(lastRowTile);
                    return newRowTile;
                }).ToList();
                rows.Add(newRow);
            }

            return rows;
        }

        private static List<List<char>> ConcatTiles(List<List<Tile>> tiles)
        {
            return tiles.SelectMany(row =>
                Enumerable.Range(1, row[0].Data.Count - 2).Select(i =>
                    row.SelectMany(t => t.Data[i].Take(t.Data[i].Count - 1).Skip(1)).ToList()
                )
            ).ToList();
        }

        private static (bool, List<List<char>>) FindMonsters(List<List<char>> img)
        {
            var monster = new List<List<char>>
            {
                "                  # ".ToList(),
                "#    ##    ##    ###".ToList(),
                " #  #  #  #  #  #   ".ToList()
            };

            var found = false;
            var image = img.Select(r => new List<char>(r)).ToList();

            for (var i = 0; i < image.Count - monster.Count + 1; i++)
            {
                for (var j = 0; j < image[i].Count - monster[0].Count + 1; j++)
                {
                    var isMonster = true;
                    for (var k = 0; k < monster.Count && isMonster; k++)
                    {
                        for (var l = 0; l < monster[k].Count && isMonster; l++)
                        {
                            if (monster[k][l] == '#' && image[i + k][j + l] != '#')
                            {
                                isMonster = false;
                            }
                        }
                    }

                    if (isMonster)
                    {
                        found = true;
                        for (var k = 0; k < monster.Count; k++)
                        {
                            for (var l = 0; l < monster[k].Count; l++)
                            {
                                if (monster[k][l] == '#')
                                {
                                    image[i + k][j + l] = 'O';
                                }
                            }
                        }
                    }
                }
            }

            return (found, image);
        }

        private static int Part2(ICollection<Tile> tiles)
        {
            var rows = ArrangeTiles(tiles);
            var image = new Tile
            {
                Data = rows.SelectMany(row =>
                    Enumerable.Range(1, row[0].Data.Count - 2).Select(i =>
                        row.SelectMany(t => t.Data[i].Take(t.Data[i].Count - 1).Skip(1)).ToList()
                    )
                ).ToList()
            };

            var (found, imageWithMonsters) = FindMonsters(image.Data);
            for (var i = 0; i < 3 && !found; i++)
            {
                image.RotateRight();
                (found, imageWithMonsters) = FindMonsters(image.Data);
            }

            if (!found)
            {
                image.RotateRight();
                image.FlipUpDown();
                (found, imageWithMonsters) = FindMonsters(image.Data);
            }

            for (var i = 0; i < 3 && !found; i++)
            {
                image.RotateRight();
                (found, imageWithMonsters) = FindMonsters(image.Data);
            }

            return found ? imageWithMonsters.SelectMany(row => row).Count(c => c == '#') : -1;
        }

        static void Main(string[] args)
        {
            var tiles = GetTiles();
            Console.WriteLine(Part1(tiles));
            Console.WriteLine(Part2(tiles));
        }
    }
}
