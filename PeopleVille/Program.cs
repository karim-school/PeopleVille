using System.Runtime.Loader;
using SimulationEngine;

namespace PeopleVille;

internal static class Program
{
    private static int Ticks = 0;
    
    private static void Main(string[] args)
    {
        var extensions = LoadExtensions();

        foreach (var extension in extensions)
        {
            extension.OnPreLoad();
        }
        
        WorldCreationOptions options = new();
        WorldManager.World = WorldManager.OnWorldCreation(options) ?? new World();
        
        WorldManager.World.EnqueueEvent(() =>
        {
            foreach (var person in WorldManager.People)
            {
                Console.WriteLine($"Found person with ID {person.ID}; Balance: {person.Cash:0.00}, Items: {string.Join(", ", person.Items)}, Hair color: #{person.Appearance.HairColorFormatted}, Skin color: #{person.Appearance.SkinColorFormatted}");
                // foreach (var items in inhabitant.Items)
                // {
                //     Console.WriteLine("  " + items);
                // }
            }
        });
        
        try
        {
            ItemRegistry.Load("items.json");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        foreach (var extension in extensions)
        {
            extension.OnLoad();
        }
        
        WorldManager.World.WorldTick += () =>
        {
            Console.WriteLine("Tick");

            var people = WorldManager.People.ToArray();
            
            if (Random.Shared.NextDouble() <= 0.5)
            {
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

            if (Random.Shared.NextDouble() < 0.5)
            {
                var randomPerson = people[Random.Shared.Next(people.Length)];
                var intent = new SeekingBuyItemIntent(randomPerson, ItemRegistry.GetRandom());
                randomPerson._intents.Add(intent);
                Console.WriteLine(intent.Declaration());
            }
            
            if (++Ticks == 3)
            {
                WorldManager.World.Stop();
            }
        };
        
        PersonFactory.CreatePeople(WorldManager.World, options.InitialPopulation);
        WorldManager.World.Run();
        
        WorldManager.OnWorldEnd();
    }

    private static List<IVilleExtension> LoadExtensions()
    {
        var sharedLibraries = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
            .Where(s => s.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase));
        var extensions = new List<IVilleExtension>();
        
        foreach (var file in sharedLibraries)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                var extensionTypes =
                    assembly.DefinedTypes.Where(info => info.ImplementedInterfaces.Contains(typeof(IVilleExtension)));
                
                foreach (var extensionType in extensionTypes)
                {
                    Console.WriteLine($"Found extension {extensionType.Assembly.GetName().Name}");
                    
                    try
                    {
                        var constructor = extensionType.GetConstructor([]);
                        
                        if (constructor == null)
                        {
                            Console.WriteLine($"Type {extensionType.FullName} does not have a default constructor");
                            continue;
                        }
                        
                        var extension = (constructor.Invoke([]) as IVilleExtension)!;
                        
                        extensions.Add(extension);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to load extension: " + extensionType.Assembly.GetName().Name);
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load library: " + file);
                Console.WriteLine(e);
            }
        }

        return extensions;
    }
}