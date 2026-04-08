using System.Collections.ObjectModel;
using PeopleVille.Engine;

namespace PeopleVille;

public class Person(IWorld world, Guid? id = null) : IWorldInhabitant
{
    public IWorld World { get; } = world;
    
    public Guid ID { get; } = id ?? Guid.NewGuid();

    public HumanAppearance Appearance { get; } = new();
    
    public decimal Cash { get; set; } = 0M;

    internal readonly List<IIntent> MutableIntents = [];

    public IReadOnlyList<IIntent> Intents => MutableIntents.AsReadOnly();
    
    private readonly Dictionary<Item, uint> _items = new();
    
    public ReadOnlyDictionary<Item, uint> Items => _items.AsReadOnly();

    public uint AddItem(Item item, uint quantity = 1)
    {
        var count = _items.GetValueOrDefault(item, 0u);
        _items[item] = count + quantity;
        return count + quantity;
    }
}