using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

using OpenNETCF.GDIPlus;
using OpenNETCF.Runtime.InteropServices.ComTypes;
using System.IO;
using TouristGuide.util;

namespace TouristGuide.gui
{
    public class PictureSlot
    {
        private Bitmap image = null;
        private Point position;

        public Bitmap Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public Point Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public bool isEmpty()
        {

            return this.image == null || this.position.IsEmpty;
        }
    }

    public class MapPanel : Panel
    {
        private static int ARROW_WIDTH = 30;
        private static int ARROW_HEIGHT = 6;
        private static Color ARROW_COLOR = Color.FromArgb(0x7fff0303);

        private static Color TARGET_LINE_COLOR = Color.FromArgb(0x7f0000ff);

        private Bitmap positionMarkerImg;
        private Bitmap off_screen;
        private Point targetPoint;
        private bool directionLineVisible;
        public bool DirectionLineVisible { set { this.directionLineVisible = value; } }
        private Hashtable pictureSlots;
        public Hashtable PictureSlots { get { return this.pictureSlots;  } }

        // rotation angle
        private double rotationAngle;


        public MapPanel()
        {
            // Gets the image from the global resources
            this.positionMarkerImg = global::TouristGuide.Properties.Resources.positionMarkerImg;
            // initialize picture slots hash table
            this.pictureSlots = new Hashtable();
            createPictureSlots();
            this.directionLineVisible = false;
            this.rotationAngle = 0;
        }

        public void showDirectionLine(Point targetPoint)
        {
            this.targetPoint = targetPoint;
            this.directionLineVisible = true;
        }

        public void setRotation(double rotationAngle)
        {
            this.rotationAngle = rotationAngle;
        }

        public void hideDirectionLine()
        {
            this.directionLineVisible = false;
        }

        // Initialize picture slots for parts of view to display.
        private void createPictureSlots()
        {
            this.pictureSlots[new Point(1, 1)] = new PictureSlot();
            this.pictureSlots[new Point(0, 1)] = new PictureSlot();
            this.pictureSlots[new Point(0, 2)] = new PictureSlot();
            this.pictureSlots[new Point(1, 2)] = new PictureSlot();
            this.pictureSlots[new Point(2, 2)] = new PictureSlot();
            this.pictureSlots[new Point(2, 1)] = new PictureSlot();
            this.pictureSlots[new Point(2, 0)] = new PictureSlot();
            this.pictureSlots[new Point(1, 0)] = new PictureSlot();
            this.pictureSlots[new Point(0, 0)] = new PictureSlot();
        }

        public new Size Size
        {
            get { return base.Size; }
            set {
                base.Size = value;
                this.off_screen = new Bitmap(this.Width, this.Height);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do not paint background - map rendering is faster and looks better
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics g = Graphics.FromImage(this.off_screen);
            g.Clear(this.BackColor);
            drawMap(g);
           
            // GDI Plus
            IntPtr hdc = g.GetHdc();
            using (GraphicsPlus graphics = new GraphicsPlus(hdc))
            {
                if (this.directionLineVisible)
                    drawDirectionLinePlus(graphics);
                if (this.rotationAngle != 0)
                    drawRotatedArrow(graphics);
            }
            g.ReleaseHdc(hdc);
            
            drawPositionMarkerImg(g);

            //Bitmap rotated = rotate();
            //g.DrawImage(rotated, this.Width / 2, this.Height / 2);


            e.Graphics.DrawImage(this.off_screen, 0, 0);
        }

        private void drawRotatedArrow(GraphicsPlus graphics)
        {
            Debug.WriteLine("drawRotatedArrow: " + this.rotationAngle, this.ToString());

            Point origin = new Point(this.Width / 2, this.Height / 2);

            int arWidthHalf = ARROW_WIDTH / 2;
            int arHeightHalf = ARROW_HEIGHT / 2;

            Point[] arrowPoints = {
                new Point(origin.X, origin.Y - arWidthHalf),
                new Point(origin.X - arHeightHalf, origin.Y - arWidthHalf + 2 * arHeightHalf),
                new Point(origin.X + arHeightHalf, origin.Y - arWidthHalf + 2 * arHeightHalf),
                new Point(origin.X, origin.Y + arWidthHalf)
            };
            Debug.WriteLine("drawRotatedArrow: points "
                + PointUtil.pointsStr(arrowPoints), this.ToString());
            
            PointMath.RotatePoints(arrowPoints, origin, this.rotationAngle);
            Debug.WriteLine("drawRotatedArrow: points "
                + PointUtil.pointsStr(arrowPoints), this.ToString());

            graphics.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);

            PenPlus pen = new PenPlus(Color.FromArgb(0x7fff0303), 3);
            pen.SetEndCap(LineCap.LineCapRound);
            pen.SetStartCap(LineCap.LineCapRound);


            graphics.DrawLine(pen, new GpPoint(arrowPoints[0].X, arrowPoints[0].Y),
                                   new GpPoint(arrowPoints[3].X, arrowPoints[3].Y));
            graphics.DrawLine(pen, new GpPoint(arrowPoints[0].X, arrowPoints[0].Y),
                                   new GpPoint(arrowPoints[1].X, arrowPoints[1].Y));
            graphics.DrawLine(pen, new GpPoint(arrowPoints[0].X, arrowPoints[0].Y),
                                   new GpPoint(arrowPoints[2].X, arrowPoints[2].Y));
            pen.Dispose();
           
        }

        private void drawMap(Graphics g)
        {
            foreach (PictureSlot pSlot in this.PictureSlots.Values)
            {
                if (!pSlot.isEmpty())
                {
                    g.DrawImage(pSlot.Image, pSlot.Position.X, pSlot.Position.Y);
                }
            }
        }

        private void drawDirectionLinePlus(GraphicsPlus graphics)
        {            
            graphics.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);

            PenPlus pen = new PenPlus(TARGET_LINE_COLOR, 3);
            pen.SetEndCap(LineCap.LineCapRound);
            pen.SetStartCap(LineCap.LineCapRound);

            graphics.DrawLine(pen, this.Width / 2, this.Height / 2, targetPoint.X, targetPoint.Y);
            pen.Dispose();
        }

        private void drawDirectionLine(Graphics g)
        {
            using (Pen p = new Pen(Color.Red))
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                g.DrawLine(p, this.Width / 2, this.Height / 2, targetPoint.X, targetPoint.Y);
            }
        }

        private void drawPositionMarkerImg(Graphics g)
        {
            // image size
            int width = positionMarkerImg.Size.Width;
            int height = positionMarkerImg.Size.Height;
            // image position
            int x = this.Width / 2 - width / 2;
            int y = this.Height / 2 - height / 2;
            // set image attributes
            ImageAttributes attrs = new ImageAttributes();
            attrs.SetColorKey(this.positionMarkerImg.GetPixel(0, 0), this.positionMarkerImg.GetPixel(0, 0));
            //attrs.SetColorKey(Color.Transparent, Color.Transparent);
            Rectangle rDest = new Rectangle(x, y, width, height);
            // Draws the image
            g.DrawImage(this.positionMarkerImg, rDest, 0, 0, width, height, GraphicsUnit.Pixel, attrs);
        }

        private void drawPositionMarkerEllipse(Graphics g)
        {
            int x = this.Width / 2;
            int y = this.Height / 2;
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                g.FillEllipse(brush, new Rectangle(x-5, y-5, 10, 10));
                brush.Color = Color.Red;
                g.FillEllipse(brush, new Rectangle(x-3, y-3, 6, 6));
            }
        }

    }
}
