namespace StoreVille;

public class Store(string name)
{
    public StoreInventory Inventory { get; } = new();

    internal readonly List<StoreTransaction> _transactions = [];
    
    public Guid Id { get; } = Guid.NewGuid();
    
    public string Name { get; } = name;
    
    public IReadOnlyList<StoreTransaction> Transactions => _transactions.AsReadOnly();

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Store store && store.Id == Id;
    }
}