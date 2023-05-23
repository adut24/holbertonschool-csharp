using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/// <summary> Represents the possible actions on images. </summary>
static class ImageProcessor
{
    /// <summary> Inverts the colors on the images contained in the array <paramref name="filenames"/>.</summary>
    /// <param name="filenames">Array of path to the images.</param>
    public static void Inverse(string[] filenames)
    {
        int numberImages = filenames.Length;

        Task[] tasks = new Task[numberImages];

        for (int i = 0; i < numberImages; i++)
        {
            int index = i;
            tasks[i] = Task.Run(() => InvertColors(filenames[index]));
        }

        Task.WaitAll(tasks);
    }

    /* Inverts the colors of the image. */
   private static void InvertColors(string filename)
    {
        const string basePath = "images/";
        string baseName = Path.GetFileName(filename);
        string[] nameSplit = baseName.Split('.');
        string name = nameSplit[0];
        string extension = nameSplit[1];

        using (Bitmap image = new Bitmap(Path.Combine(basePath, baseName)))
        {
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            int stride = imageData.Stride;
            int height = image.Height;

            byte[] imageBytes = new byte[stride * height];
            int lengthBytes = imageBytes.Length;

            /* Copies all the bytes from the image to the new array to modify them */
            Marshal.Copy(imageData.Scan0, imageBytes, 0, lengthBytes);

            /* Inverts the pixels */
            for (int y = 0; y < height; y++)
            {
                int rowOffset = y * stride;

                for (int x = 0; x < stride; x += 4)
                {
                    int offset = rowOffset + x;
                    imageBytes[offset] = (byte)(255 - imageBytes[offset]);
                    imageBytes[offset + 1] = (byte)(255 - imageBytes[offset + 1]);
                    imageBytes[offset + 2] = (byte)(255 - imageBytes[offset + 2]);
                }
            }

            /* Returns all the new bytes to the bitmap to create the new image */
            Marshal.Copy(imageBytes, 0, imageData.Scan0, lengthBytes);
            image.UnlockBits(imageData);

            image.Save($"{name}_inverse.{extension}");
        }
    }
}
