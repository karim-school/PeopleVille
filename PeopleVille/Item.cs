namespace PeopleVille;

public class Item(string name, ItemCategory category, string? description = null)
{
    public string Name { get; } = name;
    public ItemCategory Category { get; } = category;
    public string? Description { get; } = description;

    public override string ToString()
    {
        return Description == null
            ? $"{Name} ({Category})"
            : $"{Name} ({Category}); {Description}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Category, Name, Description);
    }

    public override bool Equals(object? obj)
    {
        return obj is Item item && item.GetHashCode() == GetHashCode();
    }
}