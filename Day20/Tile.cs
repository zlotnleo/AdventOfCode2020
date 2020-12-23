using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    public enum Side
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Tile
    {
        private static readonly IReadOnlyList<Side> AllSides = new List<Side>
            {Side.Up, Side.Down, Side.Left, Side.Right};

        public int Id;
        public List<List<char>> Data;

        public Dictionary<Side, Tile> Neighbours = AllSides.ToDictionary(side => side, _ => (Tile) null);

        private static int GetSideCode(IEnumerable<char> side)
        {
            return side.Aggregate(0, (code, c) => code << 1 | (c == '#' ? 1 : 0));
        }

        private (int, int) GetSide(Side side)
        {
            var sideData = (side switch
            {
                Side.Up => Data.First(),
                Side.Down => Data.Last(),
                Side.Left => Data.Select(r => r.First()),
                Side.Right => Data.Select(r => r.Last()),
                _ => null
            })?.ToArray();
            return (GetSideCode(sideData), GetSideCode(sideData?.Reverse()));
        }

        public void FindNeighbours(ICollection<Tile> allTiles)
        {
            Neighbours = new Dictionary<Side, Tile>();
            foreach (var side in AllSides)
            {
                var sideCode = GetSide(side).Item1;
                Neighbours[side] = allTiles.SingleOrDefault(otherTile =>
                    otherTile.Id != Id
                    && AllSides.Any(otherTileSide =>
                    {
                        var (otherSideCode1, otherSideCode2) =
                            otherTile.GetSide(otherTileSide);
                        return sideCode == otherSideCode1 ||
                               sideCode == otherSideCode2;
                    }));
            }
        }

        public void RotateRight()
        {
            Data = Enumerable.Range(0, Data[0].Count).Select(i =>
                Data.Select(row => row[i]).Reverse().ToList()
            ).ToList();

            var tmp = Neighbours[Side.Up];
            Neighbours[Side.Up] = Neighbours[Side.Left];
            Neighbours[Side.Left] = Neighbours[Side.Down];
            Neighbours[Side.Down] = Neighbours[Side.Right];
            Neighbours[Side.Right] = tmp;
        }

        public void RotateLeft()
        {
            Data = Enumerable.Range(0, Data[0].Count).Select(i =>
                Data.Select(row => row[i]).ToList()
            ).Reverse().ToList();

            var tmp = Neighbours[Side.Up];
            Neighbours[Side.Up] = Neighbours[Side.Right];
            Neighbours[Side.Right] = Neighbours[Side.Down];
            Neighbours[Side.Down] = Neighbours[Side.Left];
            Neighbours[Side.Left] = tmp;
        }

        public void Rotate180()
        {
            Data.ForEach(row => row.Reverse());
            Data.Reverse();

            var tmp = Neighbours[Side.Up];
            Neighbours[Side.Up] = Neighbours[Side.Down];
            Neighbours[Side.Down] = tmp;

            tmp = Neighbours[Side.Left];
            Neighbours[Side.Left] = Neighbours[Side.Right];
            Neighbours[Side.Right] = tmp;
        }

        public void FlipUpDown()
        {
            Data.Reverse();

            var tmp = Neighbours[Side.Up];
            Neighbours[Side.Up] = Neighbours[Side.Down];
            Neighbours[Side.Down] = tmp;
        }

        public void FlipLeftRight()
        {
            Data.ForEach(row => row.Reverse());

            var tmp = Neighbours[Side.Left];
            Neighbours[Side.Left] = Neighbours[Side.Right];
            Neighbours[Side.Right] = tmp;
        }

        public void OrientLeft(Tile otherTile)
        {
            var side = Neighbours.Single(neighbour => neighbour.Value == otherTile).Key;
            switch (side)
            {
                case Side.Left:
                    break;
                case Side.Down:
                    RotateRight();
                    break;
                case Side.Right:
                    Rotate180();
                    break;
                case Side.Up:
                    RotateLeft();
                    break;
            }

            var (leftCode1, leftCode2) = GetSide(Side.Left);
            var (otherTileRightCode, _) = otherTile.GetSide(Side.Right);

            if (otherTileRightCode == leftCode2)
            {
                FlipUpDown();
            }
            else if(otherTileRightCode != leftCode1)
            {
                throw new InvalidDataException();
            }
        }

        public void OrientUp(Tile otherTile)
        {
            var side = Neighbours.Single(neighbour => neighbour.Value == otherTile).Key;
            switch (side)
            {
                case Side.Up:
                    break;
                case Side.Left:
                    RotateRight();
                    break;
                case Side.Down:
                    Rotate180();
                    break;
                case Side.Right:
                    RotateLeft();
                    break;
            }

            var (upCode1, upCode2) = GetSide(Side.Up);
            var (otherTileDownCode, _) = otherTile.GetSide(Side.Down);

            if (otherTileDownCode == upCode2)
            {
                FlipLeftRight();
            }
            else if(otherTileDownCode != upCode1)
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {
            return string.Join("\n", Data.Select(row => new string(row.ToArray())));
        }
    }
}
