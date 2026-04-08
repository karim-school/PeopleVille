namespace PeopleVille.Engine;

public interface IWorld
{
    IEnumerable<IWorldInhabitant> Inhabitants { get; }
    
    IWorldInhabitant? GetInhabitant(Guid id);
    
    bool AddInhabitant(IWorldInhabitant inhabitant);
    
    bool RemoveInhabitant(IWorldInhabitant inhabitant);
}