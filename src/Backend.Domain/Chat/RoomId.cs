namespace Backend.Domain.Chat;

public readonly record struct RoomId
{
    public const int MaxLength = 64;

    public string Value { get; }

    private RoomId(string value)
    {
        Value = value;
    }

    public static RoomId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("RoomId is required.", nameof(value));
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"RoomId cannot exceed {MaxLength} characters.");
        }

        return new RoomId(normalized);
    }

    public override string ToString()
    {
        return Value;
    }
}
