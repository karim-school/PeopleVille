namespace PeopleVille.Engine;

public interface IIntent
{
    public IWorldInhabitant Inhabitant { get; }
    
    string Declaration();
}