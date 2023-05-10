﻿/// <summary>
/// Represents operations on vectors.
/// </summary>
class VectorMath
{
    /// <summary>
    /// Calculates the cross product of a vector.
    /// </summary>
    /// <param name="vector1">First vector</param>
    /// <param name="vector2">Second vector</param>
    /// <returns>A vector with -1 or the cross product.</returns>
    public static double[] CrossProduct(double[] vector1, double[] vector2)
    {
        if (vector1.Length != 3 || vector2.Length != 3)
            return new double[] { -1 };

        return new double[]
        {
            (vector1[1] * vector2[2]) - (vector1[2] * vector2[1]),
            (vector1[2] * vector2[0]) - (vector1[0] * vector2[2]),
            (vector1[0] * vector2[1]) - (vector1[1] * vector2[0])
        };
    }
}
