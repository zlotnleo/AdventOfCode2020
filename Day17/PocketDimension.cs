using System;
using System.Collections.Generic;
using System.Linq;

namespace Day17
{
    public class PocketDimension
    {
        private class CoordEqualityComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x == null || y == null)
                {
                    return x == null && y == null;
                }

                if (x.Length != y.Length)
                {
                    return false;
                }

                for (var i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(int[] obj)
            {
                const int prime = 31;
                return obj.Aggregate(0, (hash, element) => hash + prime * element);
            }
        }

        private static readonly CoordEqualityComparer EqualityComparer = new CoordEqualityComparer();
        private int dimensions;
        private ISet<int[]> active;

        public PocketDimension(int dimensions, bool[][] initSlice)
        {
            this.dimensions = dimensions;

            active = new HashSet<int[]>(EqualityComparer);
            for (var i = 0; i < initSlice.Length; i++)
            {
                for (var j = 0; j < initSlice[i].Length; j++)
                {
                    if (initSlice[i][j])
                    {
                        var coord = new int[dimensions];
                        coord[0] = j;
                        coord[1] = i;
                        active.Add(coord);
                    }
                }
            }
        }

        private ICollection<int[]> GetRegionAround(int[] coord)
        {
            var coords = new List<int[]>();

            // Each n is <dimension> ternary digits representing coordinate offset
            for (var n = 0; n < (int) Math.Pow(3, dimensions); n++)
            {
                var newCoord = (int[]) coord.Clone();
                var k = n;
                for (var i = 0; i < dimensions; i++)
                {
                    newCoord[i] = newCoord[i] + (k % 3 - 1);
                    k /= 3;
                }

                coords.Add(newCoord);
            }

            return coords;
        }

        private int CountActiveNeighbours(int[] coord)
        {
            return GetRegionAround(coord).Count(aroundCoord =>
                !EqualityComparer.Equals(aroundCoord, coord) && active.Contains(aroundCoord)
            );
        }

        public void Step()
        {
            var newActive = new HashSet<int[]>(EqualityComparer);
            var coords = active.SelectMany(GetRegionAround).Distinct(EqualityComparer).ToList();

            foreach (var coord in coords)
            {
                var neighbours = CountActiveNeighbours(coord);
                var isActive = active.Contains(coord);
                if (isActive && (neighbours == 2 || neighbours == 3)
                    || !isActive && neighbours == 3)
                {
                    newActive.Add(coord);
                }
            }

            active = newActive;
        }

        public int CountActive()
        {
            return active.Count;
        }
    }
}
