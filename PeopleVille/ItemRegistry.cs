using Newtonsoft.Json;

namespace PeopleVille;

public static class ItemRegistry
{
    private static List<Item> ItemsRegistry { get; } = [];

    public static IReadOnlyList<Item> Items { get; } = ItemsRegistry.AsReadOnly();

    public static Item GetRandom()
    {
        return ItemsRegistry.Count == 0
            ? throw new Exception("Registry has no items")
            : ItemsRegistry[Random.Shared.Next(ItemsRegistry.Count)];
    }

    public static IEnumerable<Item> GetRandom(int quantity)
    {
        if (ItemsRegistry.Count == 0)
        {
            throw new Exception("Registry has no items");
        }

        for (var i = 0; i < quantity; i++)
        {
            yield return ItemsRegistry[Random.Shared.Next(ItemsRegistry.Count)];
        }
    }

    public static void Add(Item item)
    {
        ItemsRegistry.Add(item);
    }

    public static void Add(IEnumerable<Item> items)
    {
        ItemsRegistry.AddRange(items);
    }

    public static bool Remove(Item item)
    {
        return ItemsRegistry.Remove(item);
    }

    public static List<Item> Load(string file)
    {
        try
        {
            var json = File.ReadAllText(file);
            var items = JsonConvert.DeserializeObject<List<Item>>(json);
            
            if (items == null)
            {
                throw new Exception("Failed to parse items: Expected list of items");
            }
            
            items.ForEach(item => ItemsRegistry.Add(item));
            return items;
        } catch (Exception e)
        {
            throw new Exception("Failed to load items: " + file, e);
        }
    }
}