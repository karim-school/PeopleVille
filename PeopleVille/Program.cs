namespace PeopleVille;

internal static class Program
{
    private static void Main(string[] args)
    {
        try
        {
            ItemRegistry.Load("items.json");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        ExtensionLoader.GetExtensions();
        ExtensionLoader.PreLoad();
        
        if (!WorldManager.Worlds.Any())
        {
            WorldManager.CreateWorld();
        }
        
        WorldManager.Worlds[0].EnqueueEvent(world =>
        {
            foreach (var person in world.People)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName} with ID {person.ID};" +
                                  $" Balance: {person.Cash:0.00}," +
                                  $" Items: {string.Join(", ", person.Items)}," +
                                  $" Hair color: #{person.Appearance.HairColorFormatted}," +
                                  $" Skin color: #{person.Appearance.SkinColorFormatted}");
            }
        });
        
        ExtensionLoader.Load();
        
        WorldManager.Worlds[0].WorldTick += DefaultWorldBehavior.AddItem;
        WorldManager.Worlds[0].WorldTick += DefaultWorldBehavior.SellItem;
        WorldManager.Worlds[0].WorldTick += DefaultWorldBehavior.BuyItem;
        
        var ticks = 0;
        WorldManager.Worlds[0].WorldTick += world =>
        {
            if (++ticks > 100)
            {
                world.Stop();
            }
        };
        
        WorldManager.Worlds[0].Start();
    }
}