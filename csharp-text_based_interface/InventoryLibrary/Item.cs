using System.Collections.Generic;
using System.Text.Json.Serialization;
using InventoryLibrary;

/// <summary>
/// Represents an item.
/// </summary>
public class Item : BaseClass
{
    private List<string> _tags;

    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the item.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the price of the item.
    /// </summary>
    [JsonPropertyName("price")]
    public float Price { get; set; }

    /// <summary>
    /// Gets or sets the tags of an item.
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags
    {
        get => _tags;
        set=> _tags = value;
    }

    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public Item() {}

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Name of the item</param>
    public Item(string name) : base()
    {
        Name = name;
        _tags = new List<string>();
    }
}
