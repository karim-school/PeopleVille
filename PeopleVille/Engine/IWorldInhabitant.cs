namespace PeopleVille.Engine;

public interface IWorldInhabitant
{
    IWorld World { get; }
    
    Guid ID { get; }
    
    IReadOnlyList<IIntent> Intents { get; }
}