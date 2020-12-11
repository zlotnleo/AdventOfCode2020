using System.Collections.Generic;
using System.Linq;

namespace Day11
{
    public enum Seat
    {
        Empty,
        Occupied,
        Floor,
        Invalid
    }

    public class WaitingArea
    {
        private List<List<Seat>> seats;
        private NextSeatStrategy nextSeatStrategy;

        public WaitingArea(NextSeatStrategy nextSeatStrategy, List<List<Seat>> seats)
        {
            this.nextSeatStrategy = nextSeatStrategy;
            this.seats = seats;
        }

        public WaitingArea Step() =>
            new WaitingArea(
                nextSeatStrategy,
                seats.Select((row, i) =>
                    row.Select((_, j) =>
                        nextSeatStrategy.GetNextSeat(seats, i, j)
                    ).ToList()
                ).ToList()
            );

        public bool Same(WaitingArea other) =>
            seats.Zip(other.seats).All(tuple => tuple.First!.SequenceEqual(tuple.Second!));

        public int Count(Seat seat) =>
            seats.Select(row => row.Count(s => s == seat)).Sum();
    }
}
