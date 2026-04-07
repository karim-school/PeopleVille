using PeopleVille;

namespace StoreVille;

public class StoreTransaction(Store store, Person person, Item item, uint quantity, decimal pricePerUnit)
{
    public Store Store { get; } = store;
    
    public Person Person { get; } = person;
    
    public Item Item { get; } = item;

    public uint Quantity { get; } = quantity;

    public decimal PricePerUnit { get; } = pricePerUnit;
    
    public decimal TotalPrice => Quantity * PricePerUnit;
}