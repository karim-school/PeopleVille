using PeopleVille.Engine;

namespace PeopleVille;

public static class ItemTransaction
{
    public static IEnumerable<SellItemIntent> GetSellIntents(IWorld world)
    {
        return world.Inhabitants
            .Where(x => x is Person)
            .Cast<Person>()
            .SelectMany(x => x.Intents)
            .Where(x => x is SellItemIntent)
            .Cast<SellItemIntent>();
    }
    
    public static IEnumerable<SellItemIntent> GetSellIntents(IWorld world, Item item)
    {
        return GetSellIntents(world).Where(x => Equals(x.Item, item));
    }
    
    public static IEnumerable<BuyItemIntent> GetBuyIntents(IWorld world)
    {
        return world.Inhabitants
            .Where(x => x is Person)
            .Cast<Person>()
            .SelectMany(x => x.Intents)
            .Where(x => x is BuyItemIntent)
            .Cast<BuyItemIntent>();
    }
    
    public static IEnumerable<BuyItemIntent> GetBuyIntents(IWorld world, Item item)
    {
        return GetBuyIntents(world).Where(x => Equals(x.Item, item));
    }

    public static void Signal(SellItemIntent intent)
    {
        Console.WriteLine(intent.Declaration());
        
        var originalQuantity = intent.Quantity;
        
        // TODO
        
        if (intent.Quantity == 0)
        {
            return;
        }
        
        (intent.Inhabitant as Person)?.MutableIntents.Add(intent);
        
        if (intent.Quantity != originalQuantity)
        {
            Console.WriteLine(intent.Declaration());
        }
    }
    
    public static void Signal(BuyItemIntent intent)
    {
        Console.WriteLine(intent.Declaration());
        
        var world = intent.Inhabitant.World;
        var sellIntents = GetSellIntents(world, intent.Item)
            .Where(x => x.Inhabitant != intent.Inhabitant)
            .Where(x => (x.Inhabitant as Person)?.Items.GetValueOrDefault(intent.Item, 0u) > 0)
            .OrderBy(x => x.PricePerUnit);
        var originalQuantity = intent.Quantity;
        var cashSpent = 0M;
        foreach (var sellIntent in sellIntents)
        {
            var toBuy = intent.Quantity;
            var canBuy = (uint)((intent.Inhabitant as Person)?.Cash / sellIntent.Price)!;
            
            if (canBuy < sellIntent.Quantity)
            {
                toBuy = canBuy;
            }
            
            if (canBuy == 0 || (intent.Budget.HasValue && cashSpent + sellIntent.PricePerUnit * toBuy > intent.Budget))
            {
                var budget = intent.Budget.HasValue ? intent.Budget.Value - cashSpent : (intent.Inhabitant as Person)?.Cash;
                Console.WriteLine($"{intent.Inhabitant.ID} wants to buy {toBuy}x from {sellIntent.Inhabitant.ID}, but the price ({sellIntent.PricePerUnit * toBuy}) is out of their budget ({budget}).");
                break;
            }

            var sellerItemCount = (sellIntent.Inhabitant as Person)?.Items[sellIntent.Item] ?? 0;
            var quantity = Math.Min(Math.Min(sellerItemCount, sellIntent.Quantity), toBuy);

            if (quantity == 0)
            {
                continue;
            }

            var price = FulfillTransaction(intent, sellIntent, quantity);
            intent.Quantity -= quantity;
            cashSpent += price;
            
            Console.WriteLine($"{intent.Inhabitant.ID} bought {quantity}x {intent.Item.Name} from {sellIntent.Inhabitant.ID} for {price:0.00}");
            
            if (intent.Quantity == 0)
            {
                return;
            }
        }
        
        (intent.Inhabitant as Person)?.MutableIntents.Add(intent);

        if (intent.Quantity != originalQuantity)
        {
            Console.WriteLine(intent.Declaration());
        }
    }

    private static decimal FulfillTransaction(BuyItemIntent buyIntent, SellItemIntent sellIntent, uint quantity)
    {
        var price = sellIntent.PricePerUnit * quantity;
        
        (sellIntent.Inhabitant as Person)?.RemoveItem(buyIntent.Item, quantity);
        (buyIntent.Inhabitant as Person)?.AddItem(buyIntent.Item, quantity);
        (buyIntent.Inhabitant as Person)?.Cash -= price;

        if (quantity < sellIntent.Quantity)
        {
            sellIntent.Price = sellIntent.PricePerUnit * (sellIntent.Quantity - quantity);
            sellIntent.Quantity -= quantity;
        }
        else
        {
            (sellIntent.Inhabitant as Person)?.MutableIntents.Remove(sellIntent);
        }

        return price;
    }
}