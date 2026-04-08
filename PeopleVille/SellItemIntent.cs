using PeopleVille.Engine;

namespace PeopleVille;

public class SellItemIntent(IWorldInhabitant inhabitant, Item item, decimal price, uint quantity = 1) : IIntent
{
    public IWorldInhabitant Inhabitant { get; } = inhabitant;

    public Item Item { get; } = item;
    
    public decimal Price { get; } = price;
    
    public uint Quantity { get; } = quantity;
    
    public string Declaration()
    {
        return $"{Inhabitant.ID} wants to sell {Quantity}x {Item.Name} for {Price:0.00}";
    }
}