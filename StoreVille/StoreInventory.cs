using PeopleVille;

namespace StoreVille;

public class StoreInventory
{
    private readonly Dictionary<Item, ItemStock> _itemStock = new();

    public ItemStock this[Item item] => _itemStock[item];

    public void AddItem(Item item, decimal price)
    {
        if (_itemStock.TryGetValue(item, out var stock))
        {
            stock.Price = price;
            return;
        }
        
        _itemStock.Add(item, new ItemStock(item, price));
    }

    public bool RemoveItem(Item item)
    {
        return _itemStock.Remove(item);
    }
}