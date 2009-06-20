using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TouristGuide.util
{
    public static class ImageUtil
    {
        public static Image rotateImage(Image inputImg, double rotationAngle, Point rotationPoint)
        {
            ////Corners of the image
            //PointF[] rotationPoints = { new PointF(0, 0),
            //                            new PointF(inputImg.Width, 0),
            //                            new PointF(0, inputImg.Height),
            //                            new PointF(inputImg.Width, inputImg.Height)};

            ////Rotate the corners
            //PointMath.RotatePoints(rotationPoints, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f), degreeAngle);

            ////Get the new bounds given from the rotation of the corners
            ////(avoid clipping of the image)
            //Rectangle bounds = PointMath.GetBounds(rotationPoints);

            // An empy bitmap to draw the rotated image
            Bitmap rotated = new Bitmap(inputImg.Width, inputImg.Height);

            //using (Graphics g = Graphics.FromImage(rotatedBitmap))
            //{
            //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //    //Transformation matrix
            //    Matrix m = new Matrix();
            //    m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));
            //    m.Translate(-bounds.Left, -bounds.Top, MatrixOrder.Append); //shift to compensate for the rotation

            //    g.Transform = m;
            //    g.DrawImage(inputImg, 0, 0);
            //}

            return (Image)rotated;
        }

    }
}
