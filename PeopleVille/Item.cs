namespace PeopleVille;

public class Item(string name, ItemCategory category, string? description = null)
{
    public string Name { get; set; } = name;
    public ItemCategory Category { get; } = category;
    public string? Description { get; set; } = description;

    public override string ToString()
    {
        if (description == null)
        {
            return $"{Name} ({Category})";
        }
        return $"{Name} ({Category}); {Description}";
    }
}