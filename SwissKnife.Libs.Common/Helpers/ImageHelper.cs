using System.Drawing;
using System.Text;

namespace SwissKnife.Libs.Common;

public class ImageHelper
{
    /// <summary>
    /// Creates thumbail of image using image path at the same directory
    /// </summary>
    /// <param name="imagePath"></param>
    public static void CreateThumbnail(string imagePath)
    {
        Image myThumbnail150;
        object obj = new();
        Image.GetThumbnailImageAbort myCallback = new(ThumbnailCallback);

        using (Image imagesize = Image.FromFile(imagePath))
        {
            Bitmap bitmapNew = new(imagesize);
            if (imagesize.Width < imagesize.Height)
            {
                myThumbnail150 = bitmapNew.GetThumbnailImage(50 * imagesize.Width / imagesize.Height, 50, myCallback, IntPtr.Zero);
            }
            else
            {
                myThumbnail150 = bitmapNew.GetThumbnailImage(50, imagesize.Height * 50 / imagesize.Width, myCallback, IntPtr.Zero);
            }
        }

        //Create a new directory name ThumbnailImage
        Directory.CreateDirectory(new FileInfo(imagePath).Directory.FullName + "\\ThumbnailImage");
        //Save image in TumbnailImage folder
        myThumbnail150.Save(new FileInfo(imagePath).Directory.FullName + "\\ThumbnailImage\\" + new FileInfo(imagePath).Name, System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    /// <summary>
    /// Converts image to base64 encoded string using image path
    /// </summary>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    public static string ConvertToBase64(string imagePath)
    {
        string base64String = null;

        if (File.Exists(imagePath))
        {
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
        }
        return base64String;
    }

    /// <summary>
    /// Converts plain text to Base64 encoding
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    private static bool ThumbnailCallback() => false;
}
