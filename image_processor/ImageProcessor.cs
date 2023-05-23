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
    public static void Inverse(string[] filenames) => Parallel.ForEach(filenames, InvertColors);

    /* Inverts the colors of the image. */
    private static void InvertColors(string filename)
    {
        const string basePath = "images/";
        string baseName = Path.GetFileNameWithoutExtension(filename);
        string extension = Path.GetExtension(filename);

        using (Bitmap image = new Bitmap(Path.Combine(basePath, baseName + extension)))
        {
            int height = image.Height;
            Rectangle rect = new Rectangle(0, 0, image.Width, height);
            BitmapData imageData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            int stride = imageData.Stride;

            byte[] pixels = new byte[stride * height];
            int imageLength = pixels.Length;

            /* Copies all the integers from the image to the new array to modify them */
            Marshal.Copy(imageData.Scan0, pixels, 0, imageLength);

            /* Inverts the pixels */
            for (int i = 0; i < imageLength; i += 4)
            {
                pixels[i] ^= 0xFF;
                pixels[i + 1] ^= 0xFF;
                pixels[i + 2] ^= 0xFF;
            }

            /* Returns all the new integers to the bitmap to create the new image */
            Marshal.Copy(pixels, 0, imageData.Scan0, imageLength);
            image.UnlockBits(imageData);

            image.Save($"{baseName}_inverse{extension}");
        }
    }
}
