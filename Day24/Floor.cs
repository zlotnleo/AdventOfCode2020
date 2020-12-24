using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    public class Floor
    {
        public HashSet<(int, int)> BlackTiles;

        private static readonly ICollection<(int dx, int dy)> Offsets = new[]
        {
            (1, 0), (0, 1), (-1, 1), (-1, 0), (0, -1), (1, -1)
        };

        public Floor(ICollection<(int, int)> blackTiles)
        {
            BlackTiles = new HashSet<(int, int)>(blackTiles);
        }

        private HashSet<(int, int)> GetUpdateRegion()
        {
            var region = new HashSet<(int, int)>();
            foreach (var (x, y) in BlackTiles)
            {
                region.Add((x, y));
                foreach (var (dx, dy) in Offsets)
                {
                    region.Add((x + dx, y + dy));
                }
            }

            return region;
        }

        private int CountBlackNeighbours((int x, int y) coord)
        {
            var (x, y) = coord;
            return Offsets.Count(offset => BlackTiles.Contains((x + offset.dx, y + offset.dy)));
        }

        public void Step()
        {
            var newTiles = new HashSet<(int, int)>();
            var coords = GetUpdateRegion();

            foreach (var coord in coords)
            {
                var neighbours = CountBlackNeighbours(coord);
                var isBlack = BlackTiles.Contains(coord);
                if (isBlack && (neighbours == 1 || neighbours == 2)
                    || !isBlack && neighbours == 2)
                {
                    newTiles.Add(coord);
                }
            }

            BlackTiles = newTiles;
        }
    }
}
