using SimulationEngine;

namespace PeopleVille;

public class Person(IWorld world) : IWorldInhabitant
{
    public delegate void BuyItemHandler(ItemEnum item, int quantity, Person seller);
    
    public delegate void SellItemHandler(ItemEnum item, int quantity, Person buyer);
    
    public event BuyItemHandler? BuyItem;
    
    public event BuyItemHandler? SellItem;
    
    private readonly Dictionary<ItemEnum, (int, decimal?)> _items = new();
    
    public IWorld World { get; } = world;
    
    public Guid ID { get; } = Guid.NewGuid();

    public string FirstName { get; set; } = "";
    
    public string LastName { get; set; } = "";

    public string Name => FirstName + " " + LastName;

    public HumanAppearance Appearance { get; } = new();
    
    public decimal Cash { get; set; } = 0M;

    public IEnumerable<(ItemEnum ItemType, int Quantity, decimal? SellPrice)> Items =>
        _items.Select(x => (x.Key, x.Value.Item1, x.Value.Item2));
    
    internal int AddItem(ItemEnum itemType, int quantity = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        
        (int Quantity, decimal? SellPrice) current = _items.GetValueOrDefault(itemType, (0, null));
        current.Quantity += quantity;
        _items[itemType] = current;
        return current.Quantity;
    }

    internal void SetSellPrice(ItemEnum itemType, decimal? price)
    {
        (int Quantity, decimal? SellPrice) current = _items.GetValueOrDefault(itemType, (0, null));
        current.SellPrice = price;
        _items[itemType] = current;
    }

    internal static bool MakeTransaction(Person buyer, Person seller, ItemEnum itemType, int quantity, decimal totalPrice)
    {
        lock (seller._items) // TODO: Lock buyer cash as well (maybe through transactional class)
        {
            if (buyer.Cash < totalPrice ||
                !seller._items.TryGetValue(itemType, out (int Quantity, decimal? SellPrice) item) ||
                item.Quantity < quantity)
            {
                return false;
            }
            
            item.Quantity -= quantity;
            seller._items[itemType] = item;
            buyer.Cash -= totalPrice;
            return true;
        }
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