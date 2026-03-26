using SimulationEngine;

namespace PeopleVille;

public class Person(IWorld world) : IWorldInhabitant
{
    public IWorld World { get; } = world;
    
    public Guid ID { get; } = Guid.NewGuid();

    public HumanAppearance Appearance { get; } = new();
    
    public decimal Cash { get; set; } = 0M;
}