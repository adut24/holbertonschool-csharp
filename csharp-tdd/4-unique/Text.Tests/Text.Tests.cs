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
		/// Tests with a normal string.
		/// </summary>
		[Test]
		public void TestWithNormalString()
		{
			int result = Str.UniqueChar("Hello");
			Assert.AreEqual(1, result);
		}

		/// <summary>
		/// Tests with an empty string.
		/// </summary>
		[Test]
		public void TestWithEmptyString()
		{
			int result = Str.UniqueChar("");
			Assert.AreEqual(-1, result);
		}

		/// <summary>
		/// Tests with a repeating character only string.
		/// </summary>
		[Test]
		public void TestWithRepeatingCharacter()
		{
			int result = Str.UniqueChar("aaaaaaaa");
			Assert.AreEqual(-1, result);
		}

		/// <summary>
		/// Tests with a repeating character only string.
		/// </summary>
		[Test]
		public void TestWithRepeatingCharacterAndNonRepeating()
		{
			int result = Str.UniqueChar("aaaaaaaab");
			Assert.AreEqual(8, result);
		}
	}
}
