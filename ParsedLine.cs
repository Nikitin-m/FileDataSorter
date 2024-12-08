namespace FileDataSorter;

internal readonly record struct ParsedLine : IComparable<ParsedLine>
{
    public string ReaderLine { get; }
    public int SeparatorPositionIndex { get; }
    public int Number { get; }
    public ReadOnlySpan<char> Text => ReaderLine.AsSpan().Slice(SeparatorPositionIndex + 2);

    public ParsedLine(string line)
    {
        ReaderLine = line;
        var span = ReaderLine.AsSpan();
        SeparatorPositionIndex = span.IndexOf('.');
        Number = int.Parse(span.Slice(0, SeparatorPositionIndex));
    }

    public int CompareTo(ParsedLine other)
    {
        var stringComparison = Text.CompareTo(other.Text, StringComparison.Ordinal);
        if (stringComparison != 0) 
            return stringComparison;
        return Number.CompareTo(other.Number);
    }
}