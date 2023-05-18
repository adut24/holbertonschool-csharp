using System.Collections.Generic;
using System.Drawing;
using System.Threading;

/// <summary> Represents the possible actions on images. </summary>
class ImageProcessor
{
    /// <summary> Inverts the colors on the images contained in the array <paramref name="filenames"/>.</summary>
    /// <param name="filenames">Array of path to the images.</param>
    public static void Inverse(string[] filenames) => InvertColorsInParallel(filenames);

    /* Inverts the images' colors in different threads and waits for the end of each threads. */
    private static void InvertColorsInParallel(string[] filenames)
    {
        List<Thread> threads = new List<Thread>();

        foreach (string filename in filenames)
        {
            Thread thread = new Thread(InvertColors);
            thread.Start(filename);
            threads.Add(thread);
        }

        foreach (Thread thread in threads)
            thread.Join();
    }

    /* Inverts the colors of the image. */
    private static void InvertColors(object obj)
    {
        string filename = (string)obj;
        string[] nameSplit = filename.Split('.');
        string fileName = nameSplit[0];
        string extension = nameSplit[1];

        Bitmap image = new Bitmap(filename);

        /* Inverts the pixels */
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Color originalColor = image.GetPixel(x, y);
                Color invertedColor = Color.FromArgb(
                    255 - originalColor.R,
                    255 - originalColor.G,
                    255 - originalColor.B
                );
                image.SetPixel(x, y, invertedColor);
            }
        }

        string invertedImagePath = fileName + "_inverse." + extension;
        
        image.Save(invertedImagePath);
        /* Free the image */
        image.Dispose();
    }
}
