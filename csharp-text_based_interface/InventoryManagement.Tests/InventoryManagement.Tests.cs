using System;
using System.IO;
using ConsoleExectution;
using NUnit.Framework;

namespace InventoryManagement.Tests
{
    [TestFixture]
    public class Tests
    {
        private InventoryManager _inventoryManager;

        [SetUp]
        public void Setup()
        {
            Directory.SetCurrentDirectory("../../../../InventoryManager/");
            _inventoryManager = new InventoryManager();
        }

        [Test]
        public void ShowInstance_ValidClassNameAndObjectId_OutputStringMatchesExpected()
        {
            // Arrange
            const string className = "Item";
            const string objectId = "b07ab8e4-85cb-44d4-a016-2aae7ba49604";
            const string expectedOutput = "{\"name\":\"computer\"," +
            "\"description\":\"personal computer\",\"price\":132.5," +
            "\"tags\":[\"tag1\",\"tag2\"],\"id\":\"b07ab8e4-85cb-44d4-a016-2aae7ba49604\"," +
            "\"date_created\":\"2023-07-16T11:30:44.559012+02:00\",\"date_updated\":\"2023-07-16T11:55:11.211639+02:00\"}";

            // Act
            string input = "Show " + className + " " + objectId;
            string output = RunConsoleWithInput(input);

            // Assert
            Assert.AreEqual(expectedOutput, output.Trim());
        }

        private string RunConsoleWithInput(string input)
        {
            using (StringWriter consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);
                using (StringReader consoleInput = new StringReader(input))
                {
                    Console.SetIn(consoleInput);
                    _inventoryManager.LaunchConsole(false);
                    return consoleOutput.ToString();
                }
            }
        }

    }
}
