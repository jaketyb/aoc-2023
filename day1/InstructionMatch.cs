public class InstructionMatch
{
    public InstructionMatch(NumberString match, int index)
    {
        Match = match;
        Index = index;
    }

    public NumberString Match { get; }
    public int Index { get; }
}
