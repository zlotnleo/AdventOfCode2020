namespace Day8
{
    public class Instruction
    {
        public enum Operation
        {
            Nop,
            Acc,
            Jmp
        }

        public int Address;
        public Operation? Opcode;
        public int Offset;

        public Instruction()
        {
        }

        public Instruction(Instruction other)
        {
            Address = other.Address;
            Opcode = other.Opcode;
            Offset = other.Offset;
        }
    }
}
