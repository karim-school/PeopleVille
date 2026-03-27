namespace PeopleVille;

public static class TransactionalEvents
{
    public delegate void CashForItemTransactionHandler(Person buyer, Person seller, ItemEnum item, int quantity,
        decimal pricePerUnit);
    
    public static event CashForItemTransactionHandler? CashForItemTransaction;

    internal static void OnCashForItemTransaction(Person buyer, Person seller, ItemEnum item, int quantity,
        decimal pricePerUnit)
    {
        CashForItemTransaction?.Invoke(buyer, seller, item, quantity, pricePerUnit);
    }
}