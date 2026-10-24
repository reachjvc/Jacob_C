namespace Jacob_C.Web.Features.Examples;

public sealed class ExampleDomain
{
    private ExampleDomain()
    {
    }

    private ExampleDomain(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public const int MaxNameLength = 100;

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public static ExampleDomain? Create(string? name)
    {
        var normalizedName = name?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedName) || normalizedName.Length > MaxNameLength)
        {
            return null;
        }

        return new ExampleDomain(Guid.NewGuid(), normalizedName);
    }
}
