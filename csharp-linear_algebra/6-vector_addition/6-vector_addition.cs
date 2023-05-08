using System;

/// <summary>
/// Represents operations on vectors.
/// </summary>
class VectorMath
{
	/// <summary>
	/// Adds two vector together.
	/// </summary>
	/// <param name="vector1"></param>
	/// <param name="vector2"></param>
	/// <returns></returns>
	public static double[] Add(double[] vector1, double[] vector2)
	{
		if (vector1.Length != 2 && vector1.Length != 3 && vector2.Length != 2 && vector2.Length && vector1.Length != vector2.Length)
			return -1;

		double[] sum = new double[];

		for (int i = 0; i < vector1.Length; i++)
		{
			sum[i] = vector1[i] + vector2[i];
		}
		return sum;
	}
}
