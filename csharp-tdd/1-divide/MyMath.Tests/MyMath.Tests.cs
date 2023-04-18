using MyMath;
using NUnit.Framework;

namespace Tests
{
	/// <summary>
	/// Tests different case for the <see cref="Matrix"/> class.
	/// </summary>
	public class Tests
	{
		/// <summary>
		///	Tests a normal case of divide.
		/// </summary>
		[Test]
		public void TestWithNormalDivide()
		{
			int[,] matrix = new int[,]
			{
				{1, 2, 3},
				{4, 5, 6},
				{7, 8, 9}
			};
			int[,] result = Matrix.Divide(matrix, 2);
			int[,] expectedResult = new int[,]
			{
				{0, 1, 1},
				{2, 2, 3},
				{3, 4, 4}
			};
			Assert.AreEqual(expectedResult, result);
		}

		/// <summary>
		/// Tests when the number to divide to is 0.
		/// </summary>
		[Test]
		public void TestWithDivideByZero()
		{
			int[,] matrix = new int[,]
			{
				{1, 2, 3},
				{4, 5, 6},
				{7, 8, 9}
			};
			int[,] result = Matrix.Divide(matrix, 0);
			Assert.AreEqual(null, result);
		}

		/// <summary>
		/// Tests when the matrix is null.
		/// </summary>
		[Test]
		public void TestWithMatrixNull()
		{
			int[,] result = Matrix.Divide(null, 9);
			Assert.AreEqual(null, result);
		}
	}
}
