using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TouristGuide.util
{
    public static class PointMath
    {
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static Point RotatePoint(Point pnt, double degreeAngle)
        {
            return RotatePoint(pnt, new Point(0, 0), degreeAngle);
        }

        public static Point RotatePoint(Point pnt, Point origin, double degreeAngle)
        {
            double radAngle = DegreeToRadian(degreeAngle);

            Point newPoint = new Point();

            double deltaX = pnt.X - origin.X;
            double deltaY = pnt.Y - origin.Y;

            newPoint.X = (int)(origin.X + (Math.Cos(radAngle) * deltaX - Math.Sin(radAngle) * deltaY));
            newPoint.Y = (int)(origin.Y + (Math.Sin(radAngle) * deltaX + Math.Cos(radAngle) * deltaY));

            return newPoint;
        }

        public static void RotatePoints(Point[] pnts, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], degreeAngle);
            }
        }

        public static void RotatePoints(Point[] pnts, Point origin, double degreeAngle)
        {
            for (int i = 0; i < pnts.Length; i++)
            {
                pnts[i] = RotatePoint(pnts[i], origin, degreeAngle);
            }
        }

        public static Rectangle GetBounds(Point[] pnts)
        {
            RectangleF boundsF = GetBoundsF(pnts);
            return new Rectangle((int)Math.Round(boundsF.Left),
                                 (int)Math.Round(boundsF.Top),
                                 (int)Math.Round(boundsF.Width),
                                 (int)Math.Round(boundsF.Height));
        }

        public static RectangleF GetBoundsF(Point[] pnts)
        {
            float left = pnts[0].X;
            float right = pnts[0].X;
            float top = pnts[0].Y;
            float bottom = pnts[0].Y;

            for (int i = 1; i < pnts.Length; i++)
            {
                if (pnts[i].X < left)
                    left = pnts[i].X;
                else if (pnts[i].X > right)
                    right = pnts[i].X;

                if (pnts[i].Y < top)
                    top = pnts[i].Y;
                else if (pnts[i].Y > bottom)
                    bottom = pnts[i].Y;
            }

            return new RectangleF(left,
                                  top,
                                 (float)Math.Abs(right - left),
                                 (float)Math.Abs(bottom - top));
        }
    }

}
