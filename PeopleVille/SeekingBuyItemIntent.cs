namespace PeopleVille;

public class SeekingBuyItemIntent(Person person, Item item, uint quantity = 1, decimal? budget = null) : IIntent
{
    public Person Person { get; } = person;

    public Item Item { get; } = item;
    
    public uint Quantity { get; } = quantity;
    
    public decimal? Budget { get; } = budget;
    
    public string Declaration()
    {
        return Budget.HasValue
            ? $"{Person.ID} is seeking to buy {Quantity}x {Item.Name} for {Budget:0.00}"
            : $"{Person.ID} is seeking to buy {Quantity}x {Item.Name}";
    }
}