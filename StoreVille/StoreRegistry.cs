using Newtonsoft.Json;

namespace StoreVille;

public static class StoreRegistry
{
    private static readonly List<Store> StoresRegistry = [];

    public static IReadOnlyList<Store> Stores { get; } = StoresRegistry.AsReadOnly();

    public static void Add(Store store)
    {
        StoresRegistry.Add(store);
    }
    
    public static void Remove(Store store)
    {
        StoresRegistry.Remove(store);
    }
    
    public static List<Store> Load(string file)
    {
        try
        {
            var json = File.ReadAllText(file);
            var stores = JsonConvert.DeserializeObject<List<Store>>(json);
            
            if (stores == null)
            {
                throw new Exception("Failed to parse stores: Expected list of stores");
            }
            
            stores.ForEach(store => StoresRegistry.Add(store));
            return stores;
        } catch (Exception e)
        {
            throw new Exception("Failed to load stores: " + file, e);
        }
    }
}