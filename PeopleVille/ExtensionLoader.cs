using System.Runtime.Loader;

namespace PeopleVille;

public static class ExtensionLoader
{
    private static readonly List<IVilleExtension> Extensions = [];
    
    internal static List<IVilleExtension> GetExtensions()
    {
        if (Extensions.Count != 0)
        {
            throw new InvalidOperationException("Extensions have already been loaded");
        }
        
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

        Extensions.AddRange(extensions);
        return extensions;
    }

    internal static void PreLoad()
    {
        foreach (var extension in Extensions)
        {
            extension.OnPreLoad();
        }
    }

    internal static void Load()
    {
        foreach (var extension in Extensions)
        {
            extension.OnLoad();
        }
    }
}