using NUnit.Framework;
using Text;

namespace Text.Tests
{
    /// <summary>
    /// Tests the <see cref="Str"/> class.
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Tests with a string that contains upper case.
        /// </summary>
        [Test]
        public void TestNotCaseSensitive()
        {
			bool result = Str.IsPalindrome("Racecar");
			Assert.AreEqual(true, result);
		}

		/// <summary>
		/// Tests with a string that contains only lower case.
		/// </summary>
		[Test]
		public void TestLowerCase()
		{
			bool result = Str.IsPalindrome("levels");
			Assert.AreEqual(false, result);
		}

		/// <summary>
		/// Tests with a more complex string.
		/// </summary>
		[Test]
		public void TestWithComplexString()
		{
			bool result = Str.IsPalindrome("A man, a plan, a canal: Panama.");
			Assert.AreEqual(true, result);
		}
    }
}
