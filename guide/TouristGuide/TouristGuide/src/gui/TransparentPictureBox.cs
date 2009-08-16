using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace TouristGuide.gui
{
    class TransparentPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            // set image attributes
            ImageAttributes attrs = new ImageAttributes();
            attrs.SetColorKey(Color.Transparent, Color.Transparent);
            Rectangle rDest = new Rectangle(0, 0, Image.Width, Image.Height);
            // Draws the image
            g.DrawImage(Image, rDest, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attrs);

        }
    }
}
