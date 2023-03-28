using System;
using System.Linq;

class Matrix
{
    public static int[,] Square(int[,] myMatrix)
    {
        int[] squaredMatrix = myMatrix.Cast<int>().Select(n => n * n).ToArray();
        int[,] newMatrix = new int[myMatrix.GetLength(0), myMatrix.GetLength(1)];
        Buffer.BlockCopy(squaredMatrix, 0, newMatrix, 0, squaredMatrix.Length * sizeof(int));
        return newMatrix;
    }
}
