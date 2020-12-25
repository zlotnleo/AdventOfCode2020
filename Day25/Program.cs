using System;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        private static (long, long) ReadPublicKeys()
        {
            var values = File.ReadLines("input.txt").Select(long.Parse).ToList();
            return (values[0], values[1]);
        }

        private static int GuessLoopSize(long subjectNumber, long publicKey)
        {
            int i;
            long value;
            for (i = 0, value = 1;
                value != publicKey;
                i++, value = (value * subjectNumber) % 20201227)
            {
            }

            return i;
        }

        private static long GetEncryptionKey(long subjectNumber, long loopSize)
        {
            int i;
            long value;
            for (i = 0, value = 1;
                i < loopSize;
                i++, value = (value * subjectNumber) % 20201227)
            {
            }

            return value;
        }

        private static long Part1(long pubKey1, long pubKey2)
        {
            var loopSize1 = GuessLoopSize(7, pubKey1);
            return GetEncryptionKey(pubKey2, loopSize1);
        }

        public static void Main(string[] args)
        {
            var (pubKey1, pubKey2) = ReadPublicKeys();
            Console.WriteLine(Part1(pubKey1, pubKey2));
        }
    }
}
