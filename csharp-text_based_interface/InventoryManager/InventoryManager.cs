using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using InventoryLibrary;

class Program
{
    private static readonly Dictionary<string, Type> _classes = new Dictionary<string, Type>
    {
        { "BaseClass", typeof(BaseClass) },
        { "Item", typeof(Item) },
        { "User", typeof(User) },
        { "Inventory", typeof(Inventory) }
    };
    private static JSONStorage _fileStorage;
    private delegate bool CommandHandler(string[] input);
    private static Dictionary<string, CommandHandler> _commandHandlers = new Dictionary<string, CommandHandler>
    {
        { "exit", ExitConsole },
        { "classnames", PrintClassNames },
        { "all", PrintAllObjects },
        { "create", CreateInstance },
        { "show", ShowInstance },
        { "update", UpdateInstance },
        { "delete", DeleteInstance }
    };
    private const bool COMMAND_SUCCESS = true;
    private const bool COMMAND_FAILURE = false;

    /* Entry point. */
    static void Main()
    {
        _fileStorage = new JSONStorage();
        _fileStorage.Load();
        if (Console.IsInputRedirected)
        {
            string[] inputParts = Console.In.ReadToEnd().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            RunCommand(inputParts);
        }
        else
        {
            PrintListCommands();
            RunConsole();
        }
    }

    /* Prints all the commands available. */
    private static void PrintListCommands()
    {
        Console.WriteLine("Inventory Manager");
        Console.WriteLine("-------------------------");
        Console.WriteLine("<ClassNames>: Print all class names of objects loaded in the JSON file");
        Console.WriteLine("<All>: Print all objects");
        Console.WriteLine("<All [ClassName]> Print all objects of the given ClassName");
        Console.WriteLine("<Create [ClassName]>: Create and save a new object of ClassName");
        Console.WriteLine("<Show [ClassName object_id]>: Print the string representation of the requested object");
        Console.WriteLine("<Update [ClassName object_id]>: Update the properties of the given object");
        Console.WriteLine("<Delete [ClassName object_id]>: Delete the given object from the JSON file");
        Console.WriteLine("<Exit>: Quit the application");
    }

    /* Runs the command passed in a non-interactive way. */
    private static void RunCommand(string[] input)
    {
        string command = input[0].ToLower();

        if (_commandHandlers.ContainsKey(command))
        {
            CommandHandler handler = _commandHandlers[command];
            handler(input);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid command.");
            Console.ResetColor();
        }
    }

    /* Runs the console the interactive way. */
    private static void RunConsole()
    {
        while (true)
        {
            Console.Write("$ ");
            string[] inputParts = SplitStringIgnoringDelimiters(Console.ReadLine());
            bool commandSuccess = false;

            if (inputParts.Length == 0)
                continue;

            string command = inputParts[0].ToLower();

            if (_commandHandlers.ContainsKey(command))
            {
                CommandHandler handler = _commandHandlers[command];
                commandSuccess = handler(inputParts);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Invalid command.");
                Console.ResetColor();
            }
            if (commandSuccess)
                PrintListCommands();
        }
    }

    /* Shows a string representation of a given instance. */
    private static bool ShowInstance(string[] input)
    {
        if (input.Length != 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Show [ClassName] [instance_id]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        string className = _classes.Keys.FirstOrDefault(key => key.ToLower().Equals(input[1].ToLower()));

        if (String.IsNullOrEmpty(className))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{input[1]} is not a valid object type");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        try
        {
            BaseClass obj = _fileStorage.All()[className + "." + input[2]];
            Console.WriteLine(JsonSerializer.Serialize(obj, obj.GetType()));
            return COMMAND_SUCCESS;
        }
        catch (KeyNotFoundException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{className} [{input[2]}] could not be found");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }
    }

    /* Prints the name of the classes available in the JSON file. */
    private static bool PrintClassNames(string[] input)
    {
        if (input.Length > 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: ClassNames");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        _fileStorage.All().Keys
        .Select(key => key.Split(".")[0])
        .ToList()
        .ForEach(Console.WriteLine);
        return COMMAND_SUCCESS;
    }

    /* Prints the string representation of all the objects in the JSON file or of a given type. */
    private static bool PrintAllObjects(string[] input)
    {
        if (input.Length > 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: All or All [ClassName]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        string className = null;

        if (input.Length == 2)
        {
            className = _classes.Keys.FirstOrDefault(key => key.ToLower().Equals(input[1].ToLower()));

            if (String.IsNullOrEmpty(className))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"{input[1]} is not a valid object type");
                Console.ResetColor();
                return COMMAND_FAILURE;
            }
        }

        Type objectType = null;

        if (!String.IsNullOrEmpty(className) && _classes.ContainsKey(className))
            objectType = _classes[className];

        _fileStorage.All(objectType).Values
            .Select(obj => JsonSerializer.Serialize(obj, obj.GetType()))
            .ToList()
            .ForEach(Console.WriteLine);
        return COMMAND_SUCCESS;
    }

    /* Creates an instance of a given class. */
    private static bool CreateInstance(string[] input)
    {
        string className = null;

        try
        {
            className = _classes.Keys.FirstOrDefault(key => key.ToLower().Equals(input[1].ToLower()));
        }
        catch (IndexOutOfRangeException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Create [ClassName] [attribute_name] [attribute_value] [...]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        if (String.IsNullOrEmpty(className))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{input[1]} is not a valid object type");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        string[] properties = input.Skip(2).ToArray();

        if (properties.Length < 2 && !className.Equals("BaseClass"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Create [ClassName] [attribute_name] [attribute_value] [...]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        BaseClass newObject;

        switch (className)
        {
            case "Item":
                if (!properties.Any(prop => prop.ToLower().Equals("name")))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Item requires a \"name\" attribute");
                    Console.ResetColor();
                    return COMMAND_FAILURE;
                }
                newObject = new Item(properties);
                break;
            case "User":
                if (!properties.Any(prop => prop.ToLower().Equals("name")))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("User requires a \"name\" attribute");
                    Console.ResetColor();
                    return COMMAND_FAILURE;
                }
                newObject = new User(properties[1]);
                break;
            case "Inventory":
                if (new[] { "user_id", "item_id", "quantity" }.All(prop => properties.Any(p => p.ToLower().Equals(prop))))
                    newObject = new Inventory(properties);
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Inventory requires the \"user_id\", \"item_id\" and \"quantity\" attributes");
                    Console.ResetColor();
                    return COMMAND_FAILURE;
                }
                break;
            default:
                newObject = new BaseClass();
                break;
        }
        _fileStorage.New(newObject);
        _fileStorage.Save();
        Console.WriteLine(newObject.Id);
        return COMMAND_SUCCESS;
    }

    /* Exits the console and stops the application. */
    private static bool ExitConsole(string[] _)
    {
        Environment.Exit(0);
        return COMMAND_SUCCESS;
    }

    /* Updates the attributes of a given instance. */
    private static bool UpdateInstance(string[] input)
    {
        string className = null;

        try
        {
            className = _classes.Keys.FirstOrDefault(key => key.ToLower().Equals(input[1].ToLower()));
        }
        catch (IndexOutOfRangeException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Update [ClassName] [instance_id] [attribute_name] [attribute_value] [...]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        if (String.IsNullOrEmpty(className))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{input[1]} is not a valid object type");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        BaseClass obj = null;

        try
        {
            obj = _fileStorage.All()[className + "." + input[2]];

            obj.UpdateProperties(input.Skip(3).ToArray());
            _fileStorage.Save();
            return COMMAND_SUCCESS;
        }
        catch (KeyNotFoundException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{className} [{input[2]}] could not be found");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }
    }

    /* Deletes a given instance. */
    private static bool DeleteInstance(string[] input)
    {
        if (input.Length != 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Delete [ClassName] [instance_id]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        string className = _classes.Keys.FirstOrDefault(key => key.ToLower().Equals(input[1].ToLower()));

        if (String.IsNullOrEmpty(className))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{input[1]} is not a valid object type");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }

        try
        {
            _fileStorage.All().Remove(className + "." + input[2]);
            _fileStorage.Save();
            return COMMAND_SUCCESS;
        }
        catch (KeyNotFoundException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{className} [{input[2]}] could not be found");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }
        catch (IndexOutOfRangeException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Invalid input format. Usage: Delete [ClassName] [instance_id]");
            Console.ResetColor();
            return COMMAND_FAILURE;
        }
    }

    /* Split the string by elements. */
    private static string[] SplitStringIgnoringDelimiters(string input)
    {
        string[] splitResult = Regex.Split(input, @"(\[.*?\])|("".*?"")|\s+");

        return splitResult.Where(s => !String.IsNullOrWhiteSpace(s))
                          .Select(s => s.Trim())
                          .ToArray();
    }
}
