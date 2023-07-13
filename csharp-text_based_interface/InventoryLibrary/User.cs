using InventoryLibrary;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a user.
/// </summary>
public class User : BaseClass
{
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Parameterless constructor.
    /// </summary>
    public User() {}

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name">Name of the user</param>
    public User(string name) : base() => Name = name;
}
