using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using TouristGuide.map.obj;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace TouristGuide.gui
{
    class PoiPanel : Panel
    {
        //private static int INFO_WIDTH = 140;
        //private static int INFO_HEIGHT = 70;
        //private PictureBox icon;
        //private LinkLabel title;
        //private Label descr;
        //private bool inInfoMode = false;


        private Poi poi;
        private MapPanel mapPanel;
        private Bitmap icon = global::TouristGuide.Properties.Resources.Bank;
        private Bitmap off_screen;

        private PoiDialogPanel poiDialog;

        public PoiPanel(Poi poi, MapPanel mapPanel)
        {
            this.poi = poi;
            this.mapPanel = mapPanel;
            initialize();
        }

        private void initialize()
        {
            //Image img = global::TouristGuide.Properties.Resources.Bank;

            //// initialize icon
            //this.icon = new PictureBox();
            //this.icon.Image = img;
            //this.icon.Size = img.Size;
            //this.icon.Click += new EventHandler(switchMode);
            //this.Controls.Add(this.icon);
            
            //// initialize title
            //this.title = new LinkLabel();
            //this.title.Text = this.poi.getName();
            //this.title.Location = new Point(img.Width + 3, 0);
            //this.title.Click += new EventHandler(gotoPoiBrowser);
            //this.Controls.Add(this.title);

            //// initialize description
            //this.descr = new Label();
            //this.descr.Font = new Font(FontFamily.GenericSerif, 8.0F, FontStyle.Regular);

            //this.descr.Text = poi.getDescr();
            //this.descr.Size = new Size(INFO_WIDTH - 6, INFO_HEIGHT - img.Height - 3);
            //this.descr.Location = new Point(3, 3 + img.Height);
            //this.Controls.Add(this.descr);

            // initialize this panel
            this.Width = 20;
            this.Height = 20;
            off_screen = new Bitmap(this.Width, this.Height);

        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Debug.WriteLine(" *** CLICK *** " + this.poi.getName(), ToString());

            if (poiDialog == null)
                poiDialog = new PoiDialogPanel(poi);

            int width = (int) Math.Floor(mapPanel.Width * 0.8);
            int height = (int) Math.Floor(mapPanel.Height * 0.4);
            poiDialog.Size = new Size(width, height);

            int x = mapPanel.Width / 2 - poiDialog.Width / 2;
            int y = mapPanel.Height / 2 - poiDialog.Height / 2;
            poiDialog.Location = new Point(x, y);

            mapPanel.Controls.Add(poiDialog);
            poiDialog.BringToFront();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawOffScreen();
            e.Graphics.DrawImage(this.off_screen, 0, 0);
        }

        private void drawOffScreen()
        {
            Graphics g = Graphics.FromImage(this.off_screen);
            //e.Graphics.Clear(Color.Violet);

            Image mapPanelImg = mapPanel.getOffScreenImage();
            g.DrawImage(mapPanelImg, - this.Location.X, - this.Location.Y);

            //using (Pen p = new Pen(Color.Red))
            //{
            //    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //    g.DrawLine(p, 0, 0, 20, 20);
            //    g.DrawLine(p, 0, 20, 20, 0);
            //}

            drawIcon(g);

            //// GDI Plus
            //IntPtr hdc = g.GetHdc();
            //using (GraphicsPlus graphics = new GraphicsPlus(hdc))
            //{
            //    drawIconPlus(graphics);
            //}
            //g.ReleaseHdc(hdc);
        }

        private void drawIcon(Graphics g)
        {
            // set image attributes
            ImageAttributes attrs = new ImageAttributes();
            //attrs.SetColorKey(icon.GetPixel(0, 0), icon.GetPixel(0, 0));
            attrs.SetColorKey(Color.Transparent, Color.Transparent);
            Rectangle rDest = new Rectangle(0, 0, icon.Width, icon.Height);
            // Draws the image
            g.DrawImage(icon, rDest, 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, attrs);
        }

        //private void drawIconPlus(GraphicsPlus graphics)
        //{
        //    graphics.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);

        //    MemoryStream ms = new MemoryStream();
        //    icon.Save(ms, ImageFormat.Bmp);
        //    StreamOnFile sof = new StreamOnFile(ms);
        //    BitmapPlus iconPlus = new BitmapPlus(sof);

        //    ImageAttributesPlus attrsPlus = new ImageAttributesPlus();
        //    attrsPlus.SetColorKey(Color.Transparent, Color.Transparent, ColorAdjustType.ColorAdjustTypeBitmap);

        //    GpRect destRect = new GpRect(0, 0, icon.Width, icon.Height);
            
        //    graphics.DrawImage(iconPlus, destRect, 0, 0, icon.Width, icon.Height, Unit.UnitPixel, attrsPlus);
        //}

    }
}
