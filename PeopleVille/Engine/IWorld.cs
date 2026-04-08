namespace PeopleVille.Engine;

public interface IWorld
{
    delegate void WorldTickHandler(IWorld world);
    
    event WorldTickHandler? WorldTick;
    event WorldTickHandler? WorldStart;
    event WorldTickHandler? WorldEnd;
    
    IEnumerable<IWorldInhabitant> Inhabitants { get; }

    IEnumerable<Person> People => Inhabitants.Where(x => x.GetType().IsAssignableFrom(typeof(Person))).Cast<Person>();
    
    IWorldInhabitant? GetInhabitant(Guid id);
    
    bool AddInhabitant(IWorldInhabitant inhabitant);
    
    bool RemoveInhabitant(IWorldInhabitant inhabitant);

    void EnqueueEvent(Action<IWorld> @event);
    
    void Start();
    
    void Stop();
}