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
    public partial class PenDemo : Form
    {
        PenPlus penSolid, penHatch, penSolidTrans, penSolidCustomCap, penDash, penGradient;
        SolidBrushPlus brSolid;
        HatchBrush brHatch;
        LinearGradientBrush brGrad;

        public PenDemo()
        {
            InitializeComponent();
        }

        private void PenDemo_Load(object sender, EventArgs e)
        {
            CreateObjects();
        }

        private void PenDemo_Closing(object sender, CancelEventArgs e)
        {
            DestroyObjects();
        }

        private void CreateObjects()
        {


            brSolid = new SolidBrushPlus(Color.CornflowerBlue);
            penSolid = new PenPlus(Color.Red, 10);
            penSolid.SetEndCap(LineCap.LineCapRound);
            penSolid.SetStartCap(LineCap.LineCapArrowAnchor);

            brHatch = new HatchBrush(HatchStyle.HatchStyle25Percent, 
                Color.Black, Color.White);
            penHatch = new PenPlus(brHatch, 10);

            penSolidTrans = new PenPlus(Color.FromArgb(-0x5f7f7f7f), 10);
            
            penSolidCustomCap = new PenPlus(Color.Black, 20);
            GraphicsPath path = new GraphicsPath(FillMode.FillModeAlternate);
            path.AddEllipse(-0.5f, -1.5f, 1, 3);
            CustomLineCap cap = new CustomLineCap(null, path, LineCap.LineCapFlat, 0);
            penSolidCustomCap.SetCustomEndCap(cap);
            
            penDash = new PenPlus(Color.Black, 5);
            penDash.SetDashStyle(DashStyle.DashStyleDot);
            
            brGrad = new LinearGradientBrush(
                new GpPointF(0, 0), new GpPointF(100, 100),
                Color.Black, Color.White);
            penGradient = new PenPlus(brGrad, 30);
        }

        private void DestroyObjects()
        {
            brSolid.Dispose();
            brHatch.Dispose();
            
        }

        private void Draw(Graphics graphics, GraphicsPlus g)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = sf.Alignment = System.Drawing.StringAlignment.Center;
            int deltaH = ClientRectangle.Height / 8;
            GpRectF rc = new GpRectF(0, 0, ClientRectangle.Width, deltaH);
            RectangleF rcf = new RectangleF(0, 0, ClientRectangle.Width, deltaH);

            using (SolidBrush brText = new SolidBrush(Color.Black))
            using (Font fnt = new Font("Tahoma", 9, System.Drawing.FontStyle.Bold))
            {
                penSolid.SetWidth(15);
                g.DrawLine(penSolid, 5, rcf.Top + 10, rc.Width - 10, rcf.Top + 10);
                graphics.DrawString("Solid with caps", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

                SmoothingMode mode = g.GetSmoothingMode();
                g.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);

                penSolid.SetColor(Color.Blue);
                g.DrawLine(penSolid, 5, rcf.Top + 10, rc.Width - 10, rcf.Top + 10);
                graphics.DrawString("Solid with caps and anitalising", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

                g.DrawLine(penHatch, 5, rcf.Top + 10, rc.Width - 10, rcf.Top + 10);
                graphics.DrawString("Hatched", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

                penSolidTrans.SetWidth(20);
                penSolidTrans.SetLineCap(LineCap.LineCapRound, LineCap.LineCapDiamondAnchor, DashCap.DashCapRound);
                graphics.DrawString("Solid with transparency", fnt, brText, rcf, sf);
                g.DrawLine(penSolidTrans, 15, rcf.Top + 10, rc.Width - 30, rcf.Top + 10);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

                g.SetSmoothingMode(SmoothingMode.SmoothingModeAntiAlias);
                brText.Color = Color.White;
                g.DrawLine(penSolidCustomCap, 15, rcf.Top + 15, rc.Width - 50, rcf.Top + 15);
                graphics.DrawString("Custom cap", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);
                g.SetSmoothingMode(mode);

                brText.Color = Color.Gray;
                g.DrawLine(penDash, 5, rcf.Top + 10, rc.Width - 10, rcf.Top + 10);
                graphics.DrawString("Dash (round)", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

                brText.Color = Color.White;
                g.DrawLine(penGradient, 15, rcf.Top + 20, rc.Width - 30, rcf.Top + 20);
                graphics.DrawString("Gradient brush-based", fnt, brText, rcf, sf);
                rcf.Y += deltaH; rc.Offset(0, deltaH);

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr hdc = e.Graphics.GetHdc();
            using (GraphicsPlus g = new GraphicsPlus(hdc))
                Draw(e.Graphics, g);
            e.Graphics.ReleaseHdc(hdc);
        }
    }
}