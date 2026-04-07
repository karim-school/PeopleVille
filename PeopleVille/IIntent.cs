namespace PeopleVille;

public interface IIntent
{
    public Person Person { get; }
    
    string Declaration();
}