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
        var people = world.People.ToArray();

        if (Random.Shared.NextDouble() >= 0.5) return;
        
        var randomPerson = people[Random.Shared.Next(people.Length)];
        var intent = new SellItemIntent(randomPerson, ItemRegistry.GetRandom(), new decimal(Random.Shared.NextDouble() * 90 + 10), (uint)Random.Shared.Next(1, 6));
        randomPerson.MutableIntents.Add(intent);
        Console.WriteLine(intent.Declaration());
    }

    public static void BuyItem(IWorld world)
    {
        var people = world.People.ToArray();

        if (Random.Shared.NextDouble() >= 0.5) return;
        
        var randomPerson = people[Random.Shared.Next(people.Length)];
        var intent = new BuyItemIntent(randomPerson, ItemRegistry.GetRandom(), (uint)Random.Shared.Next(1, 6));
        randomPerson.MutableIntents.Add(intent);
        Console.WriteLine(intent.Declaration());
    }
}