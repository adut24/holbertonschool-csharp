using System;
using System.Collections.Generic;
using System.Text.Json;
using InventoryLibrary;

class Program
{
    private static readonly Dictionary<string, Type> classes = new Dictionary<string, Type>
    {
        { "BaseClass", typeof(BaseClass) },
        { "Item", typeof(Item) },
        { "User", typeof(User) },
        { "Inventory", typeof(Inventory) }
    };

    private static JSONStorage fileStorage;

    static void Main()
    {
        fileStorage = new JSONStorage();
        fileStorage.Load();
        InitialPrompt();
        RunConsole();
    }

    private static void InitialPrompt()
    {
        Console.WriteLine("Inventory Manager");
        Console.WriteLine("-------------------------");
        Console.WriteLine("<ClassNames>: show all Class names of objects");
        Console.WriteLine("<All>: show all objects");
        Console.WriteLine("<All [ClassName]> show all objects of a Class name");
        Console.WriteLine("<Create [ClassName]>: create a new object of the given class name");
        Console.WriteLine("<Show [ClassName object_id]>: show the object with the corresponding id");
        Console.WriteLine("<Update [ClassName object_id]>: update the object with the corresponding id");
        Console.WriteLine("<Delete [ClassName object_id]>: delete an object of the corresponding id");
        Console.WriteLine("<Exit>");
    }

    private static void RunConsole()
    {
        while (true)
        {
            string[] input;

            Console.Write("$ ");

            input = Console.ReadLine().ToLower().Split();

            if (input.Length == 1)
            {
                switch (input[0])
                {
                    case "exit":
                        return;
                    case "classnames":
                        ClassNames();
                        return;
                    case "all":
                        AllObjects();
                    return;
                }
            }
        }
    }

    private static void ClassNames()
    {
        foreach (string ClassName in classes.Keys)
            Console.WriteLine(ClassName);
    }

    private static void AllObjects(Type type = null)
    {
        if (type == null)
        {
            foreach (BaseClass obj in fileStorage.All().Values)
                Console.WriteLine(JsonSerializer.Serialize(obj, obj.GetType()));
        }
    }
}
