namespace Day14
{
    public interface IInstruction
    {
    }

    public class SetMask : IInstruction
    {
        public string Mask;
    }

    public class SetValue : IInstruction
    {
        public long Address;
        public long Value;
    }
}
