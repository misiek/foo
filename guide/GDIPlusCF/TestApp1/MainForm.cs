using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.GDIPlus;
using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace TestApp1
{
    public partial class MainForm : Form
    {
        GdiplusStartupInput input = new GdiplusStartupInput();
        GdiplusStartupOutput output;
        PenPlus penWrite;
        //GraphicsPlus graphics;

        List<GpPointF> allPoints;
        List<GraphicsPath> allPaths;
        GraphicsPath path;

        public MainForm()
        {
            InitializeComponent();
            allPoints = new List<GpPointF>();
            allPaths = new List<GraphicsPath>();
        }

        IntPtr token;
        BitmapPlus bmp;

        private void MainForm_Load(object sender, EventArgs e)
        {
            GpStatusPlus stat = NativeMethods.GdiplusStartup(out token, input, out output);
            string bitmapPath = System.IO.Path.GetDirectoryName(GetType().Assembly.GetModules()[0].FullyQualifiedName);
            bitmapPath = System.IO.Path.Combine(bitmapPath, "test.jpg");
            StreamOnFile sf = new StreamOnFile(bitmapPath);
            bmp = new BitmapPlus(sf);
            penWrite = new PenPlus(Color.Blue, 3);

            path = new GraphicsPath(FillMode.FillModeAlternate);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Capture = true;
            AddStroke(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            AddStroke(e.X, e.Y);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            path.Clear();
            path.AddBeziers(allPoints.ToArray());
            allPaths.Add(path);
            allPoints.Clear();
            path = new GraphicsPath();
        }

        private void AddStroke(int x, int y)
        {
            allPoints.Add(new GpPointF(x, y));
            if ( allPoints.Count == 1 )
                return;
            path.AddLine(allPoints[allPoints.Count - 2], allPoints[allPoints.Count - 1]);
            using (Graphics graphics = CreateGraphics())
            {
                IntPtr hdc = graphics.GetHdc();
                using (GraphicsPlus g = new GraphicsPlus(hdc))
                {
                    SmoothingMode mode = g.GetSmoothingMode();
                    g.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);
                    g.DrawLine(penWrite, allPoints[allPoints.Count - 2], allPoints[allPoints.Count - 1]);
                    g.SetSmoothingMode(mode);
                }
                graphics.ReleaseHdc(hdc);
            }
        }

        private void TestGdiPlus(GraphicsPlus graphics)
        {
            graphics.DrawImage(bmp, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            PenPlus pen;

            graphics.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);

            pen= new PenPlus(Color.Black, 15);
            pen.SetEndCap(LineCap.LineCapRound);
            pen.SetStartCap(LineCap.LineCapRound);

            pen.SetWidth(3);

            pen.SetColor(Color.FromArgb(0x7f7f7f7f));
            pen.SetWidth(40);
            graphics.DrawLine(pen, 20, 20, ClientRectangle.Right - 20, ClientRectangle.Bottom - 20);
            graphics.DrawLine(pen, ClientRectangle.Right - 20, 20, 20, ClientRectangle.Bottom - 20);

            SmoothingMode mode = graphics.GetSmoothingMode();
            graphics.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);
            foreach(GraphicsPath p in allPaths)
                graphics.DrawPath(penWrite, p);
            graphics.SetSmoothingMode(mode);
            pen.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr hdc = e.Graphics.GetHdc();
            using (GraphicsPlus g = new GraphicsPlus(hdc))
                TestGdiPlus(g);
            e.Graphics.ReleaseHdc(hdc);

            StringFormat fmt = new StringFormat();
            fmt.Alignment = fmt.LineAlignment = System.Drawing.StringAlignment.Center;
            using (SolidBrush brText = new SolidBrush(Color.LightGray))
            using (Font font = new Font("Tahoma", 8, System.Drawing.FontStyle.Bold))
                e.Graphics.DrawString("Use stylus to write on this screen", font, brText, new Rectangle(0, 0, ClientRectangle.Width, 30), fmt);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            penWrite.Dispose();
            path.Dispose();
            //graphics.Dispose();
            NativeMethods.GdiplusShutdown(token);

        }

        private void mnuPenDemo_Click(object sender, EventArgs e)
        {
            using (PenDemo frm = new PenDemo())
                frm.ShowDialog();
        }

        private void mnuBrushDemo_Click(object sender, EventArgs e)
        {
            using (BrushDemo frm = new BrushDemo())
                frm.ShowDialog();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            allPoints.Clear();
            path.Clear();
            foreach (GraphicsPath p in allPaths)
                p.Clear();
            allPaths.Clear();
            Invalidate();
        }
    }
}