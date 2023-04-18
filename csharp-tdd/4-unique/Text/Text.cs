using System;

namespace Text
{
	/// <summary>
	/// Represents a string checker.
	/// </summary>
	public static class Str
	{
		/// <summary>
		/// Finds the index of the first non-repeating character.
		/// </summary>
		/// <param name="s">String to check</param>
		/// <returns>The index of the character or -1 if there's none.</returns>
		public static int UniqueChar(string s)
		{
			for (int i = 1; i < s.Length; i++)
			{
				if (s[i] != s[0])
					return i;
			}
			return -1;
		}
	}
}
