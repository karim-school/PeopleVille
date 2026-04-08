using PeopleVille.Engine;

namespace PeopleVille;

public static class PersonFactory
{
    public static Person CreatePerson(IWorld world, Guid? id = null)
    {
        var person = new Person(world, id);
        world.AddInhabitant(person);
        person.Cash = new decimal(Random.Shared.NextDouble() * 10000);
        
        var itemCount = Random.Shared.Next(Math.Min(ItemRegistry.Items.Count, 4)) + 1;
        foreach (var item in ItemRegistry.GetRandom(itemCount))
        {
            var itemQuantity = (uint)Random.Shared.Next(10);
            person.AddItem(item, itemQuantity);
        }

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