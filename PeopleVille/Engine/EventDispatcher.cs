namespace PeopleVille.Engine;

public sealed class EventDispatcher
{
    private readonly Queue<Action> _actions = new();

    public void Enqueue(Action action)
    {
        _actions.Enqueue(action);
    }

    public bool DispatchNext()
    {
        if (!_actions.TryDequeue(out var action))
        {
            return false;
        }
        
        action.Invoke();
        return true;
    }
}