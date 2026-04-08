namespace PeopleVille.Engine;

public class World : IWorld
{
    public event IWorld.WorldTickHandler? WorldTick;
    public event IWorld.WorldTickHandler? WorldStart;
    public event IWorld.WorldTickHandler? WorldEnd;

    protected readonly EventDispatcher<IWorld> EventDispatcher;
    protected readonly HashSet<IWorldInhabitant> MutableInhabitants = [];
    
    protected bool Running;
    protected long NextTick;

    public World()
    {
        EventDispatcher = new EventDispatcher<IWorld>(this);
    }

    public IEnumerable<IWorldInhabitant> Inhabitants => MutableInhabitants.AsReadOnly();
    
    public void Start()
    {
        Running = true;
        WorldStart?.Invoke(this);
        NextTick = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        while (Running)
        {
            while (EventDispatcher.DispatchNext())
            {
                Thread.Sleep(200);
            }
            
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now >= NextTick)
            {
                WorldTick?.Invoke(this);
                NextTick = now + Random.Shared.NextInt64(50, 750);
            }
        }
    }

    public void Stop()
    {
        Running = false;
        WorldEnd?.Invoke(this);
    }

    public void EnqueueEvent(Action<IWorld> @event)
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
}