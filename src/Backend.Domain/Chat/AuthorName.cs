namespace Backend.Domain.Chat;

public readonly record struct AuthorName
{
    public const int MaxLength = 64;

    public string Value { get; }

    private AuthorName(string value)
    {
        Value = value;
    }

    public static AuthorName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("AuthorName is required.", nameof(value));
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"AuthorName cannot exceed {MaxLength} characters.");
        }

        return new AuthorName(normalized);
    }

    public override string ToString()
    {
        return Value;
    }
}
