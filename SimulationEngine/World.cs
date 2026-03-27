namespace SimulationEngine;

public class World : IWorld
{
    public delegate void WorldTickHandler();
    
    public event WorldTickHandler? WorldTick;
    
    protected readonly EventDispatcher EventDispatcher = new();
    protected readonly HashSet<IWorldInhabitant> MutableInhabitants = [];
    
    protected bool Running;
    protected long nextTick;

    public IEnumerable<IWorldInhabitant> Inhabitants => MutableInhabitants.AsEnumerable();
    
    public void Run()
    {
        Running = true;
        nextTick = DateTimeOffset.Now.ToUnixTimeMilliseconds() + Random.Shared.NextInt64(50, 10000);
        while (Running)
        {
            while (EventDispatcher.DispatchNext())
            {
                Thread.Sleep(200);
            }
            
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now >= nextTick)
            {
                OnWorldTick();
                nextTick = now + Random.Shared.NextInt64(50, 10000);
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
        return MutableInhabitants.FirstOrDefault(x => x.ID == id);
    }

    public bool AddInhabitant(IWorldInhabitant inhabitant)
    {
        return MutableInhabitants.Add(inhabitant);
    }
    
    public bool RemoveInhabitant(IWorldInhabitant inhabitant)
    {
        return MutableInhabitants.Remove(inhabitant);
    }

    protected virtual void OnWorldTick()
    {
        WorldTick?.Invoke();
    }
}