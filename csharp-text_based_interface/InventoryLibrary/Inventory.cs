using System.Text.Json.Serialization;
using InventoryLibrary;

/// <summary>
/// Represents the inventory.
/// </summary>
public class Inventory : BaseClass
{
    /// <summary>
    /// Gets or sets the id of the user inputing the item.
    /// </summary>
    public string User_id { get; set; }

    /// <summary>
    /// Gets or sets the id of item inputed.
    /// </summary>
    public string Item_id { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public Inventory() {}

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="userId">ID of a user</param>
    /// <param name="itemId">ID of an item</param>
    /// <param name="quantity">Quantity of the item</param>
    public Inventory(string userId, string itemId, int quantity = 1) : base()
    {
        User_id = userId;
        Item_id = itemId;
        if (quantity < 0)
            quantity = 0;
        Quantity = quantity;
    }
}
