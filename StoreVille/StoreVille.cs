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
        // throw new NotImplementedException();
    }
}