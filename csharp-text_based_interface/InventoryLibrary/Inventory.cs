public class Inventory : BaseClass
{
    public string user_id;
    public string item_id;
    public int quantity;

    public Inventory(string userId, string itemId, int quantity = 1)
    {
        user_id = userId;
        item_id = itemId;
        if (quantity < 0)
            quantity = 0;
        this.quantity = quantity;
    }
}
