using System;

/// <summary>
/// Represents operations on matrixes.
/// </summary>
class MatrixMath
{
    /// <summary>
    /// Inverts a 2D matrix.
    /// </summary>
    /// <param name="matrix">Matrix to invert</param>
    /// <returns>A matrix with -1 if it's a singular matrix or not a 2D matrix or the inverted matrix.</returns>
    public static double[,] Inverse2D(double[,] matrix)
    {
        if (matrix.GetLength(0) != 2)
            return new double[,] { { -1 } };

        double determinant = Determinant(matrix);

        if (determinant == 0)
            return new double[,] { { -1 } };

        double[,] transposedMatrix = Transpose(matrix);
        transposedMatrix[1, 0] = -transposedMatrix[1, 0];
        transposedMatrix[0, 1] = -transposedMatrix[0, 1];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
                transposedMatrix[i, j] = Math.Round(transposedMatrix[i, j] / determinant, 2);
        }
        return transposedMatrix;
    }

    /* Transposes a 2D matrix. */
    private static double[,] Transpose(double[,] matrix)
    {
        double[,] transposedMatrix = new double[2, 2];

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
                transposedMatrix[j, i] = matrix[i, j];
        }
        return transposedMatrix;
    }

    /* Calculates the determinant of a 2D matrix. */
    private static double Determinant(double[,] matrix)
    {
        return Math.Round(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0], 2);
    }
}
