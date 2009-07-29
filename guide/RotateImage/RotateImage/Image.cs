
namespace AForge.Imaging
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
    using System.Diagnostics;


	/// <summary>
	/// Core functions
	/// </summary>
	public class Image
	{
		/// <summary>
		/// Check if the image is grayscale
		/// </summary>
		public static bool IsGrayscale(Bitmap bmp)
		{
			bool ret = false;

			// check pixel format
            //if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            //{
            //    ret = true;
            //    // check palette
            //    ColorPalette cp = bmp.Palette;
            //    Color c;
            //    // init palette
            //    for (int i = 0; i < 256; i++)
            //    {
            //        c = cp.Entries[i];
            //        if ((c.R != i) || (c.G != i) || (c.B != i))
            //        {
            //            ret = false;
            //            break;
            //        }
            //    }
            //}
			return ret;
		}

		/// <summary>
		/// Create and initialize grayscale image
		/// </summary>
        //public static Bitmap CreateGrayscaleImage(int width, int height)
        //{
        //    // create new image
        //    Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
        //    // set palette to grayscale
        //    SetGrayscalePalette(bmp);
        //    // return new image
        //    return bmp;
        //}

		/// <summary>
		/// Set pallete of the image to grayscale
		/// </summary>
        //public static void SetGrayscalePalette(Bitmap srcImg)
        //{
        //    // check pixel format
        //    if (srcImg.PixelFormat != PixelFormat.Format8bppIndexed)
        //        throw new ArgumentException();

        //    // get palette
        //    ColorPalette cp = srcImg.Palette;
        //    // init palette
        //    for (int i = 0; i < 256; i++)
        //    {
        //        cp.Entries[i] = Color.FromArgb(i, i, i);
        //    }
        //    // set palette back
        //    srcImg.Palette = cp;
        //}

		/// <summary>
		/// Clone image
		/// Note: It looks like Bitmap.Clone() with specified PixelFormat does not
		/// produce expected result
		/// </summary>
		public static Bitmap Clone(Bitmap src, PixelFormat format)
		{
			int width	= src.Width;
			int height	= src.Height;

			// create new image with desired pixel format
			Bitmap bmp = new Bitmap(width, height, format);

			// draw source image on the new one using Graphics
			Graphics g = Graphics.FromImage(bmp);
			g.DrawImage(src, 0, 0);
			g.Dispose();

			return bmp;
		}
		// and with unspecified PixelFormat it works strange too
		public static Bitmap Clone(Bitmap src)
		{
			return Clone(src, PixelFormat.Format24bppRgb);
		}


		/// <summary>
		/// Format an input image
		/// Convert it to 24 RGB or leave untouched if it's a grayscale image
		/// </summary>
		public static void FormatImage(ref Bitmap src)
		{
			Bitmap tmp = src;
			// convert to 24 bits per pixel
			src = Clone(tmp, PixelFormat.Format24bppRgb);
			// delete old image
			tmp.Dispose();	
		}

	}
}
