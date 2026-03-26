using SimulationEngine;

namespace PeopleVille;

internal static class Program
{
    private static void Main(string[] args)
    {
        var world = new World();
        
        const int initialPopulation = 5;
        var people = PersonFactory.CreatePeople(world, initialPopulation);
        
        world.EnqueueEvent(() =>
        {
            foreach (var person in people)
            {
                Console.WriteLine($"Skin color: {person.Appearance.SkinColorFormatted}");
                Console.WriteLine($"Hair color: {person.Appearance.HairColorFormatted}");
            }
        });

        world.Run();
    }
}