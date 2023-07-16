using NUnit.Framework;

namespace InventoryManagement.Tests
{
    public class Tests
    {
        [Test]
        public void PrintClassNames_ValidInput_PrintsClassNames()
        {
            StringWriter outputWriter = new StringWriter();
            Console.SetOut(outputWriter);

            Program.PrintClassNames(new[] { "ClassNames" });
            string output = outputWriter.ToString().Trim();

            Assert.AreEqual("BaseClass\nItem\nUser\nInventory", output);
        }
    }
}
