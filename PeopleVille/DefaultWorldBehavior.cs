using PeopleVille.Engine;

namespace PeopleVille;

public static class DefaultWorldBehavior
{
    public static void AddItem(IWorld world)
    {
        var people = world.People.ToArray();

        if (Random.Shared.NextDouble() >= 0.5) return;
        
        Console.WriteLine("Random event occurred");
                
        try
        {
            var randomItem = ItemRegistry.GetRandom();
            var randomPerson = people[Random.Shared.Next(people.Length)];
                    
            randomPerson.AddItem(randomItem);
                    
            Console.WriteLine($"Added {randomItem.Name} to person with ID {randomPerson.ID}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void SellItem(IWorld world)
    {
        var randomItem = ItemRegistry.GetRandom();
        var people = world.People.Where(x => x.Items.ContainsKey(randomItem)).ToArray();
        
        var randomPerson = people[Random.Shared.Next(people.Length)];
        var intent = new SellItemIntent(randomPerson, randomItem, new decimal(Random.Shared.NextDouble() * 90 + 10), (uint)Random.Shared.Next(1, 6));
        randomPerson.MutableIntents.Add(intent);
        Console.WriteLine(intent.Declaration());
    }

    public static void BuyItem(IWorld world)
    {
        var people = world.People.ToArray();
        
        var randomPerson = people[Random.Shared.Next(people.Length)];
        var buyIntent = new BuyItemIntent(randomPerson, ItemRegistry.GetRandom(), (uint)Random.Shared.Next(1, 6));
        
        Console.WriteLine(buyIntent.Declaration());

        var intentsToSell = world.Inhabitants.Except([randomPerson])
            .SelectMany(x => x.Intents)
            .Where(x => x is SellItemIntent sellIntent && Equals(sellIntent.Item, buyIntent.Item))
            .Cast<SellItemIntent>()
            .OrderBy(x => x.PricePerUnit)
            .ToArray();

        var cashSpent = 0M;
        var boughtQuantity = 0u;
        foreach (var sellIntent in intentsToSell)
        {
            var toBuy = buyIntent.Quantity - boughtQuantity;
            var canBuy = (uint)((buyIntent.Inhabitant as Person)?.Cash / sellIntent.Price)!;
            
            if (canBuy < sellIntent.Quantity)
            {
                toBuy = canBuy;
            }
            
            if (canBuy == 0 || (buyIntent.Budget.HasValue && cashSpent + sellIntent.PricePerUnit * toBuy > buyIntent.Budget))
            {
                var budget = buyIntent.Budget.HasValue ? buyIntent.Budget.Value - cashSpent : (buyIntent.Inhabitant as Person)?.Cash;
                Console.WriteLine($"{buyIntent.Inhabitant.ID} wants to buy {toBuy}x from {sellIntent.Inhabitant.ID}, but the price ({sellIntent.PricePerUnit * toBuy}) is out of their budget ({budget}).");
                break;
            }

            uint sellerItemCount;
            try
            {
                sellerItemCount = ((sellIntent.Inhabitant as Person)?.Items[sellIntent.Item]!).Value;
            }
            catch
            {
                continue;
            }
            
            var quantity = Math.Min(Math.Min(sellerItemCount, sellIntent.Quantity), toBuy);
            
            if (quantity >= toBuy)
            {
                var price = FulfillTransaction(buyIntent, sellIntent, quantity);
                Console.WriteLine($"{buyIntent.Inhabitant.ID} bought {quantity}x {buyIntent.Item.Name} from {sellIntent.Inhabitant.ID} for {price:0.00}");
                return;
            }
            else
            {
                var price = FulfillTransaction(buyIntent, sellIntent, quantity);
                boughtQuantity += quantity;
                cashSpent += price;
                Console.WriteLine($"{buyIntent.Inhabitant.ID} bought {quantity}x {buyIntent.Item.Name} from {sellIntent.Inhabitant.ID} for {price:0.00}");
            }
        }

        if (boughtQuantity < buyIntent.Quantity)
        {
            randomPerson.MutableIntents.Add(buyIntent);
            if (boughtQuantity > 0)
            {
                Console.WriteLine(buyIntent.Declaration());
            }
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