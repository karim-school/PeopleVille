namespace SimulationEngine;

public class World : IWorld
{
    protected readonly EventDispatcher EventDispatcher = new();
    protected readonly HashSet<IWorldInhabitant> Inhabitants = [];
    
    protected bool Running;
    
    public void Run()
    {
        Running = true;
        while (Running)
        {
            while (EventDispatcher.DispatchNext())
            {
                Thread.Sleep(200);
            }
        }
    }

    public void Stop()
    {
        Running = false;
    }

    public void EnqueueEvent(Action @event)
    {
        EventDispatcher.Enqueue(@event);
    }

    public IWorldInhabitant? GetInhabitant(Guid id)
    {
        return Inhabitants.FirstOrDefault(x => x.ID == id);
    }

    public bool AddInhabitant(IWorldInhabitant inhabitant)
    {
        return Inhabitants.Add(inhabitant);
    }
    
    public bool RemoveInhabitant(IWorldInhabitant inhabitant)
    {
        return Inhabitants.Remove(inhabitant);
    }
}