using PeopleVille;

namespace StoreVille;

public static class PersonExtensions
{
    public static StoreTransaction BuyItem(this Person person, Store store, Item item, uint quantity = 1)
    {
        try
        {
            var itemStock = store.Inventory[item];

            if (itemStock.Quantity < quantity)
            {
                throw new Exception("Store does not have enough of this item in stock");
            }
            
            var totalPrice = itemStock.Price * quantity;

            if (person.Cash < totalPrice)
            {
                throw new Exception("Person does not have enough cash to buy this quantity of the item");
            }
            
            person.Cash -= totalPrice;
            itemStock.Quantity -= quantity;
            person.AddItem(item, quantity);
            
            var transaction = new StoreTransaction(store, person, item, quantity, itemStock.Price);
            store._transactions.Add(transaction);
            return transaction;
        }
        catch (KeyNotFoundException)
        {
            throw new Exception("Store does not sell item");
        }
        catch (Exception e)
        {
            throw new Exception("Transaction failed", e);
        }
    }
}