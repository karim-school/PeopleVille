using SimulationEngine;

namespace PeopleVille;

public static class WorldManager
{
    public delegate World? WorldCreationHandler(WorldCreationOptions options);
    
    public delegate void WorldEndHandler();
    
    public static event WorldCreationHandler? WorldCreation;
    public static event WorldEndHandler? WorldEnd;

    public static World? World { get; internal set; }

    public static IEnumerable<Person> People => World?.Inhabitants
        .Where(x =>
            x.GetType().IsAssignableFrom(typeof(Person)))
        .Cast<Person>() ?? [];

    internal static World? OnWorldCreation(WorldCreationOptions options)
    {
        return WorldCreation?.Invoke(options);
    }

    internal static void OnWorldEnd()
    {
        WorldEnd?.Invoke();
    }
}