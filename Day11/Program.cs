using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        private static List<List<Seat>> ReadSeats() =>
            File.ReadLines("input.txt")
                .Select(line =>
                    line.Select(c => c switch
                        {
                            '.' => Seat.Floor,
                            '#' => Seat.Occupied,
                            'L' => Seat.Empty,
                            _ => throw new ArgumentOutOfRangeException()
                        }
                    ).ToList()
                ).ToList();

        private static int CountFixedPointOccupiedSeats(WaitingArea waitingArea)
        {
            WaitingArea oldWaitingArea;
            do
            {
                oldWaitingArea = waitingArea;
                waitingArea = waitingArea.Step();
            } while (!waitingArea.Same(oldWaitingArea));

            return waitingArea.Count(Seat.Occupied);
        }

        public static void Main(string[] args)
        {
            var seats = ReadSeats();

            var waitingArea1 = new WaitingArea(new Part1Strategy(), seats);
            Console.WriteLine(CountFixedPointOccupiedSeats(waitingArea1));

            var waitingArea2 = new WaitingArea(new Part2Strategy(), seats);
            Console.WriteLine(CountFixedPointOccupiedSeats(waitingArea2));
        }
    }
}
