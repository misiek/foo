using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

using OpenNETCF.GDIPlus;
using OpenNETCF.Runtime.InteropServices.ComTypes;

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
        private Bitmap positionMarkerImg;
        private Bitmap off_screen;
        private Point targetPoint;
        private bool directionLineVisible;
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
                    rotate(graphics);
            }
            g.ReleaseHdc(hdc);
            
            drawPositionMarkerImg(g);

            e.Graphics.DrawImage(this.off_screen, 0, 0);
        }

        private void rotate(GraphicsPlus graphics)
        {
            //Debug.WriteLine("rotate ################### " + this.rotationAngle, this.ToString());
        
            //Matrix X = new Matrix();
            //X.Rotate((float)this.rotationAngle, MatrixOrder.MatrixOrderAppend);
            //graphics.Transform(X);

            

            //move rotation point to center of image
            //g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            //rotate
            //g.RotateTransform(angle);
            ////move image back
            //g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);

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

            PenPlus pen = new PenPlus(Color.FromArgb(0x7fff0000), 3);
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
