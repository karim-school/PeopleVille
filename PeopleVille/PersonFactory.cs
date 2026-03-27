using SimulationEngine;

namespace PeopleVille;

public static class PersonFactory
{
    public static Person CreatePerson(IWorld world)
    {
        var person = new Person(world);
        world.AddInhabitant(person);
        person.FirstName = "John";
        person.LastName = "Doe";
        person.Cash = new decimal(Random.Shared.NextDouble() * 9000 + 1000);

        var allItems = Enum.GetValues<ItemEnum>();
        var itemCount = Random.Shared.Next(5);
        for (var i = 0; i < itemCount; i++)
        {
            var item = allItems[Random.Shared.Next(allItems.Length)];
            var quantity = Random.Shared.Next(20) + 1;
            person.AddItem(item, quantity);
            person.SetSellPrice(item, new decimal(Random.Shared.NextDouble() * 900 + 100));
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