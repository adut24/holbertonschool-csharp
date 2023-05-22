using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/// <summary> Represents the possible actions on images. </summary>
class ImageProcessor
{
    /// <summary> Inverts the colors on the images contained in the array <paramref name="filenames"/>.</summary>
    /// <param name="filenames">Array of path to the images.</param>
    public static void Inverse(string[] filenames) => InvertColorsInParallel(filenames);

    /* Inverts the images' colors in different threads and waits for the end of each threads. */
    private static void InvertColorsInParallel(string[] filenames)
    {
        List<Task> tasks = new List<Task>();

        foreach (string filename in filenames)
            tasks.Add(Task.Run(() => InvertColors(filename)));

        Task.WaitAll(tasks.ToArray());
    }

    /* Inverts the colors of the image. */
   private static void InvertColors(string filename)
    {
        string basename = Path.GetFileName(filename);
        string[] nameSplit = basename.Split('.');
        string name = nameSplit[0];
        string extension = nameSplit[1];

        Bitmap image = new Bitmap("images/" + basename);
        BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);

        int imageSize = imageData.Stride * image.Height;
        byte[] imageBytes = new byte[imageSize];

        /* Copies all the bytes from the image to the new array to modify them */
        Marshal.Copy(imageData.Scan0, imageBytes, 0, imageSize);

        /* Inverts the pixels */
        for (int i = 0; i < imageSize; i += 4)
        {
            imageBytes[i] = (byte)(255 - imageBytes[i]);
            imageBytes[i + 1] = (byte)(255 - imageBytes[i + 1]);
            imageBytes[i + 2] = (byte)(255 - imageBytes[i + 2]);
        }
        /* Returns all the new bytes to the bitmap to create the new image */
        Marshal.Copy(imageBytes, 0, imageData.Scan0, imageSize);
        image.UnlockBits(imageData);
        string invertedImagePath = name + "_inverse." + extension;
        image.Save(invertedImagePath);
        /* Free the image */
        image.Dispose();
    }
}
