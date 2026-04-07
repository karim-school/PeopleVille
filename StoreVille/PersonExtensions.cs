using PeopleVille;

namespace StoreVille;

public static class PersonExtensions
{
    public static StoreTransaction MakeTransaction(this Person person, Store store, Item item, uint quantity = 1)
    {
        try
        {
            store.RemoveItem(item, quantity);
            person.AddItem(item, quantity);
            return new StoreTransaction(store, person, item, quantity);
        }
        catch (Exception e)
        {
            throw new Exception("Transaction failed", e);
        }
    }
}