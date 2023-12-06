public class MapValue
{
    public MapValue(long destination, long source, long range)
    {
        DestinationStart = destination;
        SourceStart = source;
        Range = range;
    }

    public long DestinationStart { get; }
    public long SourceStart { get; }
    public long Range { get; }
}
