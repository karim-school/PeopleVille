namespace SimulationEngine;

public class World : IWorld
{
    private readonly EventDispatcher _eventDispatcher = new();
    private readonly HashSet<IWorldInhabitant> _inhabitants = [];
    
    private bool _running;
    
    public void Run()
    {
        _running = true;
        while (_running)
        {
            while (_eventDispatcher.DispatchNext())
            {
                Thread.Sleep(200);
            }

            _running = false;
        }
    }

    public void EnqueueEvent(Action @event)
    {
        _eventDispatcher.Enqueue(@event);
    }

    public IWorldInhabitant? GetInhabitant(Guid id)
    {
        return _inhabitants.FirstOrDefault(x => x.ID == id);
    }

    public bool AddInhabitant(IWorldInhabitant inhabitant)
    {
        return _inhabitants.Add(inhabitant);
    }
    
    public bool RemoveInhabitant(IWorldInhabitant inhabitant)
    {
        return _inhabitants.Remove(inhabitant);
    }
}