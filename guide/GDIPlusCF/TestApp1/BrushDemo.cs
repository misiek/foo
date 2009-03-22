using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenNETCF.GDIPlus;

namespace TestApp1
{
    public partial class BrushDemo : Form
    {
        SolidBrushPlus brSolid;
        TextureBrushPlus brTexture;
        HatchBrush brHatch;
        LinearGradientBrush brLinGrad;
        PathGradientBrush brPathGrad;

        public BrushDemo()
        {
            InitializeComponent();
        }

        private void BrushDemo_Load(object sender, EventArgs e)
        {
            CreateObjects();
        }

        private void BrushDemo_Closing(object sender, CancelEventArgs e)
        {
            DestroyObjects();
        }

        private void CreateObjects()
        {
            brSolid = new SolidBrushPlus(Color.CornflowerBlue);
            
            brHatch = new HatchBrush(HatchStyle.HatchStyle25Percent, 
                Color.Black, Color.White);
            
            string bitmapPath = System.IO.Path.GetDirectoryName(GetType().Assembly.GetModules()[0].FullyQualifiedName);
            bitmapPath = System.IO.Path.Combine(bitmapPath, "brushPattern.bmp");
            StreamOnFile sf = new StreamOnFile(bitmapPath);
            ImagePlus img = new ImagePlus(sf, false);
            brTexture = new TextureBrushPlus(img, WrapMode.WrapModeTile);
            brLinGrad = new LinearGradientBrush(new GpPointF(0, 0), 
                new GpPointF(50, 50), Color.Black, Color.White);
            
            // Create rectangular path
            GraphicsPath path = new GraphicsPath(FillMode.FillModeAlternate);
            path.AddRectangle(new GpRectF( 0, 0, ClientRectangle.Width, 
                ClientRectangle.Height / 5));
            
            // Create rectangular gradient brush
            // with red in center and black in the corners
            brPathGrad = new PathGradientBrush(path);
            brPathGrad.SetCenterColor(Color.Red);
            int count = 2;
            brPathGrad.SetSurroundColors(new Color[] { Color.Black, Color.Black }, 
                ref count);
        }

        private void DestroyObjects()
        {
            brSolid.Dispose();
            brTexture.Dispose();
            brPathGrad.Dispose();
            brLinGrad.Dispose();
            brHatch.Dispose();
        }

        private void Draw(Graphics graphics, GraphicsPlus g)
        {
            int deltaH = ClientRectangle.Height / 5;
            GpRectF rc = new GpRectF(0, 0, ClientRectangle.Width, deltaH);

            g.FillRectangle(brPathGrad, rc);
            rc.Offset(0, deltaH);

            g.FillRectangle(brSolid, rc);
            rc.Offset(0, deltaH);

            g.FillRectangle(brHatch, rc);
            rc.Offset(0, deltaH);

            g.FillRectangle(brLinGrad, rc);
            rc.Offset(0, deltaH);

            g.FillRectangle(brTexture, rc);
            rc.Offset(0, deltaH);

            Rectangle rcText = new Rectangle(0, 0, ClientRectangle.Width, deltaH);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = sf.Alignment = System.Drawing.StringAlignment.Center;

            using (SolidBrush brText =new SolidBrush(Color.White))
            using (Font fnt = new Font("Tahoma", 9, System.Drawing.FontStyle.Bold))
            {
                graphics.DrawString("PathGradient Brush", fnt, brText, rcText, sf);
                rcText.Offset(0, deltaH);

                brText.Color = Color.Black;
                graphics.DrawString("Solid Brush", fnt, brText, rcText, sf);
                rcText.Offset(0, deltaH);

                graphics.DrawString("Hatch Brush", fnt, brText, rcText, sf);
                rcText.Offset(0, deltaH);

                graphics.DrawString("LinearGradient Brush", fnt, brText, rcText, sf);
                rcText.Offset(0, deltaH);

                graphics.DrawString("Texture Brush", fnt, brText, rcText, sf);
                rcText.Offset(0, deltaH);

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr hdc = e.Graphics.GetHdc();
            using (GraphicsPlus g = new GraphicsPlus(hdc))
                Draw (e.Graphics, g);
            e.Graphics.ReleaseHdc(hdc);
        }
    }
}