using System.Runtime.Loader;
using SimulationEngine;

namespace PeopleVille;

internal static class Program
{
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
            WorldManager.World.Stop();
        });
        
        foreach (var extension in extensions)
        {
            extension.OnLoad();
        }
        
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