using SimulationEngine;

namespace PeopleVille;

public static class PersonFactory
{
    public static Person CreatePerson(IWorld world)
    {
        var person = new Person(world);
        world.AddInhabitant(person);
        return person;
    }
    
    public static Person[] CreatePeople(IWorld world, int size)
    {
        var people = new Person[size];
        
        for (var i = 0; i < size; i++)
        {
            people[i] = CreatePerson(world);
        }
        
        return people;
    }
}