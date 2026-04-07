using PeopleVille;

namespace StoreVille;

public class Store(string name)
{
    private readonly Dictionary<Item, uint> _itemStock = new();

    private readonly List<StoreTransaction> _transactions = [];
    
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Name { get; } = name;

    public IReadOnlyDictionary<Item, uint> ItemStock => _itemStock.AsReadOnly();
    
    public IReadOnlyList<StoreTransaction> Transactions => _transactions.AsReadOnly();

    public uint AddItem(Item item, uint quantity = 1)
    {
        var stock = _itemStock.GetValueOrDefault(item, 0u);
        _itemStock[item] = stock + quantity;
        return stock + quantity;
    }

    public uint RemoveItem(Item item, uint quantity = 1)
    {
        if (!_itemStock.ContainsKey(item))
        {
            throw new KeyNotFoundException($"No such item in stock: {item.Name}");
        }

        var stock = _itemStock.GetValueOrDefault(item, 0u);

        if (quantity > stock)
        {
            throw new ArgumentException($"Quantity too large: Not enough of this item in stock: {item.Name} ({quantity} > {stock})");
        }
        
        _itemStock[item] = stock - quantity;
        return stock - quantity;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Store store && store.Id == Id;
    }
}