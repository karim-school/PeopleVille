namespace SimulationEngine;

public interface IWorldInhabitant
{
    IWorld World { get; }
    
    Guid ID { get; }
}