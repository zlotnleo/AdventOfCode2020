using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        private static (int, ICollection<int?>) Read()
        {
            var lines = File.ReadLines("input.txt").ToList();
            var arrival = int.Parse(lines[0]);
            var buses = lines[1].Split(',')
                .Select(bus => bus == "x" ? (int?) null : int.Parse(bus))
                .ToList();
            return (arrival, buses);
        }

        private static int Part1(int arrival, ICollection<int> buses)
        {
            var earliestBus = buses.First();
            var waitTime = earliestBus - arrival % earliestBus;
            foreach (var bus in buses.Skip(1))
            {
                var curArrival = bus - arrival % bus;
                if (curArrival < waitTime)
                {
                    waitTime = curArrival;
                    earliestBus = bus;
                }
            }

            return earliestBus * waitTime;
        }

        private static bool Congruent(long x, long y, long modulo)
        {
            var xmod = x % modulo;
            if (xmod < 0)
            {
                xmod += modulo;
            }

            var ymod = y % modulo;
            if (ymod < 0)
            {
                ymod += modulo;
            }

            return xmod == ymod;
        }

        private static long Part2(ICollection<int?> nullableBuses)
        {
            var buses = nullableBuses.Select((bus, index) => (bus, index))
                .Where(t => t.bus != null)
                .Select(t => (bus: t.bus.Value, t.index))
                .ToList();
            buses.Sort((b1, b2) => b2.bus - b1.bus);

            // Chinese Remainder Theorem
            long timestamp = (buses[0].bus - buses[0].index) % buses[0].bus;
            long increment = buses[0].bus;
            for (var i = 0; i < buses.Count - 1; i++)
            {
                while (!Congruent(timestamp, buses[i + 1].bus - buses[i + 1].index, buses[i + 1].bus))
                {
                    timestamp += increment;
                }
                increment *= buses[i + 1].bus;
            }

            return timestamp;
        }

        public static void Main(string[] args)
        {
            var (arrival, buses) = Read();
            var availableBuses = buses.Where(b => b != null).Cast<int>().ToList();
            Console.WriteLine(Part1(arrival, availableBuses));
            Console.WriteLine(Part2(buses));
        }
    }
}
