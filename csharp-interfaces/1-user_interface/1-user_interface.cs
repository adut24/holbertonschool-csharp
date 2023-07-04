/// <summary>
/// Represents the base to build the classes.
/// </summary>
public abstract class Base
{
    /// <summary>
    /// Represents the name of the instance.
    /// </summary>
    public string name;

    /// <summary>
    /// Returns a string representation of the instance.
    /// </summary>
    /// <returns>The string representation of the instance.</returns>
    public override string ToString() => $"{name} is a {GetType()}";
}

/// <summary>
///
/// </summary>
public interface IInteractive
{
    void Interact();
}

public interface IBreakable
{
    int durability { get; set; }

    void Break();
}

public interface ICollectable
{
    bool isCollected { get; set; }

    void Collect();
}

public class TestObject : Base, IInteractive, IBreakable, ICollectable
{
    public int durability { get; set; }
    public bool isCollected { get; set; }

    public void Interact()
    {
    }

    public void Break()
    {
    }

    public void Collect()
    {
    }
}
