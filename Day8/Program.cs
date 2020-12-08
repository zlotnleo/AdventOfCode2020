using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day8
{
    class Program
    {
        private static IEnumerable<Instruction> ReadData()
        {
            var regex = new Regex("^(nop|acc|jmp) ([\\+\\-]\\d+)$", RegexOptions.Compiled | RegexOptions.Singleline);
            return File.ReadLines("input.txt").Select((line, i) =>
            {
                var match = regex.Match(line);
                return new Instruction
                {
                    Address = i,
                    Opcode = match.Groups[1].Value switch
                    {
                        "nop" => Instruction.Operation.Nop,
                        "acc" => Instruction.Operation.Acc,
                        "jmp" => Instruction.Operation.Jmp,
                        _ => null
                    },
                    Offset = int.Parse(match.Groups[2].Value)
                };
            });
        }

        private static (bool Halts, int Result) Run(List<Instruction> program)
        {
            var acc = 0;
            var pc = 0;
            var executedInstructions = new HashSet<int>();

            while (!executedInstructions.Contains(pc) && pc < program.Count)
            {
                executedInstructions.Add(pc);
                switch (program[pc].Opcode)
                {
                    case Instruction.Operation.Nop:
                        pc++;
                        break;
                    case Instruction.Operation.Acc:
                        acc += program[pc].Offset;
                        pc++;
                        break;
                    case Instruction.Operation.Jmp:
                        pc += program[pc].Offset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (pc >= program.Count, acc);
        }

        private static int Part1(List<Instruction> program)
        {
            return Run(program).Result;
        }

        private static int Part2(List<Instruction> program)
        {
            for (var i = 0; i < program.Count; i++)
            {
                var instruction = program[i];
                var newProgram = instruction.Opcode switch
                {
                    Instruction.Operation.Jmp => new List<Instruction>(program)
                    {
                        [i] = new Instruction(instruction)
                        {
                            Opcode = Instruction.Operation.Nop
                        }
                    },
                    Instruction.Operation.Nop => new List<Instruction>(program)
                    {
                        [i] = new Instruction(instruction)
                        {
                            Opcode = Instruction.Operation.Jmp,
                        }
                    },
                    _ => null
                };

                if (newProgram != null)
                {
                    var (halts, result) = Run(newProgram);
                    if (halts)
                    {
                        return result;
                    }
                }
            }
            throw new Exception("Always loops");
        }

        public static void Main(string[] args)
        {
            var program = ReadData().ToList();
            Console.WriteLine(Part1(program));
            Console.WriteLine(Part2(program));
        }
    }
}
