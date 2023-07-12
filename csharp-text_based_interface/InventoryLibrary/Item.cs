public class Item : BaseClass
{
    public string name;
    public string description;
    public float price;
    string[] tags;

    public Item(string name) => this.name = name;
}
