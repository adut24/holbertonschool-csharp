using System;
using System.IO;
using System.Reflection;
using ConsoleExectution;
using NUnit.Framework;

namespace InventoryManagement.Tests
{
    [TestFixture]
    public class Tests
    {
        private InventoryManager _inventoryManager;

        [OneTimeSetUp]
        public void Setup()
        {
            string parentDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));
            Directory.SetCurrentDirectory(Path.Combine(parentDirectory, "InventoryManager"));
            _inventoryManager = new InventoryManager();
        }

        [Test]
        public void TestShowValidInput()
        {
            const string expectedOutput = "{\"name\":\"computer\"," +
            "\"description\":\"personal computer\",\"price\":132.5," +
            "\"tags\":[\"tag1\",\"tag2\"],\"id\":\"b07ab8e4-85cb-44d4-a016-2aae7ba49604\"," +
            "\"date_created\":\"2023-07-16T11:30:44.559012+02:00\",\"date_updated\":\"2023-07-16T11:55:11.211639+02:00\"}";

            string output = RunConsoleWithInput("Show Item b07ab8e4-85cb-44d4-a016-2aae7ba49604");

            Assert.AreEqual(expectedOutput, output.Trim());
        }

        [Test]
        public void TestShowInvalidClassName()
        {
            const string expectedOutput = "Foo is not a valid object type";

            string output = RunConsoleWithInput("Show Foo b07ab8e4-85cb-44d4-a016-2aae7ba49604");

            Assert.AreEqual(expectedOutput, output.Trim());
        }

        [Test]
        public void TestShowInvalidId()
        {
            const string expectedOutput = "Item [FalseID] could not be found";

            string output = RunConsoleWithInput("Show Item FalseID");

            Assert.AreEqual(expectedOutput, output.Trim());
        }

        [Test]
        public void TestShowInvalidParameters()
        {
            const string expectedOutput = "Invalid input format. Usage: Show [ClassName] [instance_id]";

            string output = RunConsoleWithInput("Show");

            Assert.AreEqual(expectedOutput, output.Trim());
        }

        private string RunConsoleWithInput(string input)
        {
            using (StringWriter consoleOutput = new StringWriter())
            {
                StringWriter consoleError = new StringWriter();

                Console.SetOut(consoleOutput);
                Console.SetError(consoleError);

                using (StringReader consoleInput = new StringReader(input))
                {
                    Console.SetIn(consoleInput);
                    _inventoryManager.LaunchConsole(false);
                    return consoleOutput.ToString() + consoleError.ToString();
                }
            }
        }
    }
}
