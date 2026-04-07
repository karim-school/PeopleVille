using PeopleVille;
using SimulationEngine;

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
                store.AddItem(item, (uint)Random.Shared.Next(10, 51));
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
                randomPerson.MakeTransaction(randomStore, randomItem, randomQuantity);

                Console.WriteLine($"{randomPerson.ID} bought {randomQuantity}x {randomItem.Name} from {randomStore.Name}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException?.Message);
            }
        };
    }
}