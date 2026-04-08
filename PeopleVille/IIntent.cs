using PeopleVille.Engine;

namespace PeopleVille;

public interface IIntent
{
    public IWorldInhabitant Inhabitant { get; }
    
    string Declaration();
}