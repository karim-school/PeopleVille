using PeopleVille.Engine;

namespace PeopleVille;

public static class WorldManager
{
    public delegate void WorldCreationHandler(IWorld world);
    
    public delegate void WorldCreatedHandler(IWorld world);
    
    public static event WorldCreationHandler? WorldCreation;
    
    public static event WorldCreatedHandler? WorldCreated;

    internal static List<IWorld> MutableWorlds = [];

    public static IReadOnlyList<IWorld> Worlds { get; } = MutableWorlds.AsReadOnly();

    public static IWorld CreateWorld()
    {
        return CreateWorld<World>();
    }

    public static IWorld CreateWorld(WorldCreationOptions options)
    {
        return CreateWorld<World>(options);
    }
    
    public static T CreateWorld<T>() where T : IWorld, new()
    {
        return CreateWorld<T>(new WorldCreationOptions());
    }

    public static T CreateWorld<T>(WorldCreationOptions options) where T : IWorld, new()
    {
        var world = new T();
        WorldCreation?.Invoke(world);
        PersonFactory.CreatePeople(world, options.InitialPopulation);
        MutableWorlds.Add(world);
        WorldCreated?.Invoke(world);
        return world;
    }
}