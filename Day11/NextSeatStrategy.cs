using System.Collections.Generic;
using System.Linq;

namespace Day11
{
    public abstract class NextSeatStrategy
    {
        protected static Seat GetSeat(List<List<Seat>> seats, int row, int column)
        {
            if (row < 0 || row >= seats.Count || column < 0 || column >= seats[row].Count)
            {
                return Seat.Invalid;
            }

            return seats[row][column];
        }

        public abstract Seat GetNextSeat(List<List<Seat>> seats, int row, int column);
    }

    public class Part1Strategy : NextSeatStrategy
    {
        private static int CountOccupiedNeighbours(List<List<Seat>> seats, int row, int column) =>
            Enumerable.Range(-1, 3)
                .SelectMany(i => Enumerable.Range(-1, 3).Select(j => (R: i, C: j)))
                .Where(dSeat => dSeat.R != 0 || dSeat.C != 0)
                .Select(dSeat => GetSeat(seats, row + dSeat.R, column + dSeat.C))
                .Count(seat => seat == Seat.Occupied);

        public override Seat GetNextSeat(List<List<Seat>> seats, int row, int column)
        {
            var seat = seats[row][column];
            var occupied = CountOccupiedNeighbours(seats, row, column);
            return seat switch
            {
                Seat.Empty when occupied == 0 => Seat.Occupied,
                Seat.Occupied when occupied >= 4 => Seat.Empty,
                _ => seat
            };
        }
    }

    public class Part2Strategy : NextSeatStrategy
    {
        private static int CountVisibleOccupied(List<List<Seat>> seats, int row, int column)
        {
            var count = 0;
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int r, c;
                    Seat seat;
                    for (
                        r = row + i, c = column + j;
                        (seat = GetSeat(seats, r, c)) == Seat.Floor;
                        r += i, c += j
                    )
                    {
                    }

                    count += seat == Seat.Occupied ? 1 : 0;
                }
            }

            return count;
        }

        public override Seat GetNextSeat(List<List<Seat>> seats, int row, int column)
        {
            var seat = seats[row][column];
            var occupied = CountVisibleOccupied(seats, row, column);
            return seat switch
            {
                Seat.Empty when occupied == 0 => Seat.Occupied,
                Seat.Occupied when occupied >= 5 => Seat.Empty,
                _ => seat
            };
        }
    }
}
