namespace SimulationEngine;

public interface IWorld
{
    IWorldInhabitant? GetInhabitant(Guid id);
    
    bool AddInhabitant(IWorldInhabitant inhabitant);
    
    bool RemoveInhabitant(IWorldInhabitant inhabitant);
}