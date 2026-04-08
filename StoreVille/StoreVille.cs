using PeopleVille;
using PeopleVille.Engine;

namespace StoreVille;

public sealed class StoreVille : IVilleExtension
{
    public void OnPreLoad()
    {
        WorldManager.WorldCreation += (WorldCreationOptions options) =>
        {
            Console.WriteLine("StoreVille World creation");
            return new World();
        };
    }

    public void OnLoad()
    {
        try
        {
            var store = new Store("TestStore");
            StoreRegistry.Add(store);
            
            foreach (var item in ItemRegistry.Items)
            {
                store.Inventory.AddItem(item, new decimal(Random.Shared.NextDouble() * 90 + 10));
                store.Inventory[item].Quantity = (uint)Random.Shared.Next(10, 51);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        WorldManager.World!.WorldTick += () =>
        {
            if (Random.Shared.NextDouble() < 0.5)
            {
                return;
            }

            var people = WorldManager.People.ToArray();
            var randomPerson = people[Random.Shared.Next(people.Length)];
            var randomStore = StoreRegistry.Stores[Random.Shared.Next(StoreRegistry.Stores.Count)];
            var randomItem = ItemRegistry.GetRandom();
            var randomQuantity = (uint)Random.Shared.Next(1, 5);

            try
            {
                var transaction = randomPerson.BuyItem(randomStore, randomItem, randomQuantity);

                Console.WriteLine($"{transaction.Person.ID} bought {transaction.Quantity}x {transaction.Item.Name} from {transaction.Store.Name} for {transaction.TotalPrice:0.00}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }
        };
    }
}