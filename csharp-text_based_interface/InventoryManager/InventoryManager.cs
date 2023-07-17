﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using InventoryLibrary;

namespace ConsoleExectution
{
    /// <summary>
    /// Represents the console program.
    /// </summary>
    public class InventoryManager
    {
        private delegate bool CommandHandler(string[] input);
        private readonly Dictionary<string, Type> _classes;
        private readonly Dictionary<string, CommandHandler> _commandHandlers;
        private readonly JSONStorage _fileStorage;
        private const bool COMMAND_SUCCESS = true;
        private const bool COMMAND_FAILURE = false;
        private const string INVALID_COMMAND = "Invalid command.";

        /// <summary>
        /// Constructor.
        /// </summary>
        public InventoryManager()
        {
            _classes = new Dictionary<string, Type>
        {
            { "BaseClass", typeof(BaseClass) },
            { "Item", typeof(Item) },
            { "User", typeof(User) },
            { "Inventory", typeof(Inventory) }
        };
            _commandHandlers = new Dictionary<string, CommandHandler>
        {
            { "exit", ExitConsole },
            { "classnames", PrintClassNames },
            { "all", PrintAllObjects },
            { "create", CreateInstance },
            { "show", ShowInstance },
            { "update", UpdateInstance },
            { "delete", DeleteInstance }
        };
            _fileStorage = new JSONStorage();
            _fileStorage.Load();
        }

        /// <summary>
        /// Entry point of the console.
        /// </summary>
        public void LaunchConsole()
        {
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
        private void PrintListCommands()
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
        private void RunCommand(string[] input)
        {
            string command = input[0].ToLower();

            if (_commandHandlers.ContainsKey(command))
            {
                CommandHandler handler = _commandHandlers[command];
                handler(input);
            }
            else
            {
                PrintErrorMessage(INVALID_COMMAND);
            }
        }

        /* Runs the console the interactive way. */
        private void RunConsole()
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
                    PrintErrorMessage(INVALID_COMMAND);
                }
                if (commandSuccess)
                    PrintListCommands();
            }
        }

        /* Shows a string representation of a given instance. */
        private bool ShowInstance(string[] input)
        {
            if (input.Length != 3)
                return PrintErrorMessage("Invalid input format. Usage: Show [ClassName] [instance_id]");

            string className = _classes.Keys.FirstOrDefault(key => key.Equals(input[1], StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(className))
                return PrintErrorMessage($"{input[1]} is not a valid object type");

            try
            {
                BaseClass obj = _fileStorage.All()[className + "." + input[2]];
                Console.WriteLine(JsonSerializer.Serialize(obj, obj.GetType()));
                return COMMAND_SUCCESS;
            }
            catch (KeyNotFoundException)
            {
                return PrintErrorMessage($"{className} [{input[2]}] could not be found");
            }
        }

        /* Prints the name of the classes available in the JSON file. */
        private bool PrintClassNames(string[] input)
        {
            if (input.Length > 1)
                return PrintErrorMessage("Invalid input format. Usage: ClassNames");

            _fileStorage.All().Keys
            .Select(key => key.Split(".")[0])
            .ToList()
            .ForEach(Console.WriteLine);
            return COMMAND_SUCCESS;
        }

        /* Prints the string representation of all the objects in the JSON file or of a given type. */
        private bool PrintAllObjects(string[] input)
        {
            if (input.Length > 2)
                return PrintErrorMessage("Invalid input format. Usage: All or All [ClassName]");

            string className = null;
            bool classNameFound = false;

            if (input.Length == 2)
            {
                className = _classes.Keys.FirstOrDefault(key => key.Equals(input[1], StringComparison.OrdinalIgnoreCase));
                classNameFound = !string.IsNullOrEmpty(className);

                if (!classNameFound)
                    return PrintErrorMessage($"{input[1]} is not a valid object type");
            }

            Type objectType = null;

            if (classNameFound && _classes.ContainsKey(className))
                objectType = _classes[className];

            _fileStorage.All(objectType).Values
                .Select(obj => JsonSerializer.Serialize(obj, obj.GetType()))
                .ToList()
                .ForEach(Console.WriteLine);
            return COMMAND_SUCCESS;
        }

        /* Creates an instance of a given class. */
        private bool CreateInstance(string[] input)
        {
            string className = null;

            try
            {
                className = _classes.Keys.FirstOrDefault(key => key.Equals(input[1], StringComparison.OrdinalIgnoreCase));
            }
            catch (IndexOutOfRangeException)
            {
                return PrintErrorMessage("Invalid input format. Usage: Create [ClassName] [attribute_name] [attribute_value] [...]");
            }

            if (string.IsNullOrEmpty(className))
                return PrintErrorMessage($"{input[1]} is not a valid object type");

            string[] properties = input.Skip(2).ToArray();

            if (properties.Length < 2 && !className.Equals("BaseClass"))
                return PrintErrorMessage("Invalid input format. Usage: Create [ClassName] [attribute_name] [attribute_value] [...]");

            BaseClass newObject;

            switch (className)
            {
                case "Item":
                    if (!properties.Any(prop => prop.Equals("name", StringComparison.OrdinalIgnoreCase)))
                        return PrintErrorMessage("Item requires a \"name\" attribute");

                    newObject = new Item(properties);
                    break;
                case "User":
                    if (!properties.Any(prop => prop.Equals("name", StringComparison.OrdinalIgnoreCase)))
                        return PrintErrorMessage("User requires a \"name\" attribute");

                    newObject = new User(properties[1]);
                    break;
                case "Inventory":
                    if (new[] { "user_id", "item_id", "quantity" }.All(prop => properties.Any(p => p.Equals(prop, StringComparison.OrdinalIgnoreCase))))
                        newObject = new Inventory(properties);
                    else
                        return PrintErrorMessage("Inventory requires the \"user_id\", \"item_id\" and \"quantity\" attributes");
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
        private bool ExitConsole(string[] _)
        {
            Environment.Exit(0);
            return COMMAND_SUCCESS;
        }

        /* Updates the attributes of a given instance. */
        private bool UpdateInstance(string[] input)
        {
            string className = null;

            try
            {
                className = _classes.Keys.FirstOrDefault(key => key.Equals(input[1], StringComparison.OrdinalIgnoreCase));
            }
            catch (IndexOutOfRangeException)
            {
                return PrintErrorMessage("Invalid input format. Usage: Update [ClassName] [instance_id] [attribute_name] [attribute_value] [...]");
            }

            if (string.IsNullOrEmpty(className))
                return PrintErrorMessage($"{input[1]} is not a valid object type");

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
                return PrintErrorMessage($"{className} [{input[2]}] could not be found");
            }
        }

        /* Deletes a given instance. */
        private bool DeleteInstance(string[] input)
        {
            if (input.Length != 3)
                return PrintErrorMessage("Invalid input format. Usage: Delete [ClassName] [instance_id]");

            string className = _classes.Keys.FirstOrDefault(key => key.Equals(input[1], StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(className))
                return PrintErrorMessage($"{input[1]} is not a valid object type");

            try
            {
                _fileStorage.All().Remove(className + "." + input[2]);
                _fileStorage.Save();
                return COMMAND_SUCCESS;
            }
            catch (KeyNotFoundException)
            {
                return PrintErrorMessage($"{className} [{input[2]}] could not be found");
            }
            catch (IndexOutOfRangeException)
            {
                return PrintErrorMessage("Invalid input format. Usage: Delete [ClassName] [instance_id]");
            }
        }

        /* Splits the string by elements. */
        private string[] SplitStringIgnoringDelimiters(string input)
        {
            string[] splitResult = Regex.Split(input, @"(\[.*?\])|("".*?"")|\s+");

            return splitResult.Where(s => !String.IsNullOrWhiteSpace(s))
                              .Select(s => s.Trim())
                              .ToArray();
        }

        /* Prints the error message to stderr. */
        private bool PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ResetColor();
            return COMMAND_FAILURE;
        }
    }
}
