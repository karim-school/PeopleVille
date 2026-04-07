using PeopleVille;

namespace StoreVille;

public class ItemStock(Item item, decimal price)
{
    public Item Item { get; } = item;

    public decimal Price { get; internal set; } = price;
    
    public uint Quantity { get; internal set; } = 0;
}