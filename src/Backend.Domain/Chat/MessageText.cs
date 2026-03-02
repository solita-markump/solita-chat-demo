namespace Backend.Domain.Chat;

public readonly record struct MessageText
{
    public const int MaxLength = 2000;

    public string Value { get; }

    private MessageText(string value)
    {
        Value = value;
    }

    public static MessageText Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("MessageText is required.", nameof(value));
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"MessageText cannot exceed {MaxLength} characters.");
        }

        return new MessageText(normalized);
    }

    public override string ToString()
    {
        return Value;
    }
}
