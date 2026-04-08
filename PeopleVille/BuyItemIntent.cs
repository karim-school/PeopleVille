using PeopleVille.Engine;

namespace PeopleVille;

public class BuyItemIntent(IWorldInhabitant inhabitant, Item item, uint quantity = 1, decimal? budget = null) : IIntent
{
    public IWorldInhabitant Inhabitant { get; } = inhabitant;

    public Item Item { get; } = item;
    
    public uint Quantity { get; internal set; } = quantity;
    
    public decimal? Budget { get; internal set; } = budget;
    
    public string Declaration()
    {
        return Budget.HasValue
            ? $"{Inhabitant.ID} is seeking to buy {Quantity}x {Item.Name} for {Budget:0.00}"
            : $"{Inhabitant.ID} is seeking to buy {Quantity}x {Item.Name}";
    }
}