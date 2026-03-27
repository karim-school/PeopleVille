using SimulationEngine;

namespace PeopleVille;

public class Person(IWorld world) : IWorldInhabitant
{
    public delegate void BuyItemHandler(ItemEnum item, int quantity, Person seller);
    
    public delegate void SellItemHandler(ItemEnum item, int quantity, Person buyer);
    
    public event BuyItemHandler? BuyItem;
    
    public event BuyItemHandler? SellItem;
    
    private readonly Dictionary<ItemEnum, int> _items = new();
    
    public IWorld World { get; } = world;
    
    public Guid ID { get; } = Guid.NewGuid();

    public HumanAppearance Appearance { get; } = new();
    
    public decimal Cash { get; set; } = 0M;

    public IEnumerable<(ItemEnum ItemType, int Quantity)> Items => _items.Select(x => (x.Key, x.Value));
    
    internal int AddItem(ItemEnum itemType, int quantity = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        var current = _items.GetValueOrDefault(itemType, 0);
        current += quantity;
        _items[itemType] = current;
        return current;
    }

    internal virtual void OnBuyItem(ItemEnum item, int quantity, Person seller)
    {
        BuyItem?.Invoke(item, quantity, seller);
    }

    internal virtual void OnSellItem(ItemEnum item, int quantity, Person buyer)
    {
        SellItem?.Invoke(item, quantity, buyer);
    }
}