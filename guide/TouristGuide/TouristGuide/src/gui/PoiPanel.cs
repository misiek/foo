using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using TouristGuide.map.obj;
using System.Drawing;
using System.Diagnostics;

namespace TouristGuide.gui
{
    class PoiPanel : Panel
    {
        private static int INFO_WIDTH = 140;
        private static int INFO_HEIGHT = 70;

        private Poi poi;
        private PictureBox icon;
        private LinkLabel title;
        private Label descr;
        private bool inInfoMode = false;

        public PoiPanel(Poi poi)
        {
            this.poi = poi;
            initialize();
        }

        private void initialize()
        {
            Image img = global::TouristGuide.Properties.Resources.Bank;

            // initialize icon
            this.icon = new PictureBox();
            this.icon.Image = img;
            this.icon.Size = img.Size;
            this.icon.Click += new EventHandler(switchMode);
            this.Controls.Add(this.icon);
            
            // initialize title
            this.title = new LinkLabel();
            this.title.Text = this.poi.getName();
            this.title.Location = new Point(img.Width + 3, 0);
            this.title.Click += new EventHandler(gotoPoiBrowser);
            this.Controls.Add(this.title);

            // initialize description
            this.descr = new Label();
            this.descr.Font = new Font(FontFamily.GenericSerif, 8.0F, FontStyle.Regular);

            this.descr.Text = poi.getDescr();
            this.descr.Size = new Size(INFO_WIDTH - 6, INFO_HEIGHT - img.Height - 3);
            this.descr.Location = new Point(3, 3 + img.Height);
            this.Controls.Add(this.descr);

            // initialize this panel
            this.Size = img.Size;
            this.BackColor = Color.Yellow;
       }

        private void gotoPoiBrowser(object sender, EventArgs e)
        {
            Debug.WriteLine(" *** CLICK poi browser *** " + this.poi.getName(), ToString());
            AppContext.Instance.getPoiBrowser().preview(this.poi);
        }

        private void switchMode(object sender, EventArgs e)
        {
            Debug.WriteLine(" *** CLICK *** " + this.poi.getName(), ToString());

            if (this.inInfoMode)
            {
                Size oldSize = this.Size;
                this.Size = this.icon.Size;
                centerLocation(oldSize);
                this.inInfoMode = false;
            }
            else
            {
                Size oldSize = this.Size;
                this.Size = new Size(INFO_WIDTH, INFO_HEIGHT);
                centerLocation(oldSize);
                this.inInfoMode = true;
            }
        }

        private void centerLocation(Size oldSize)
        {
            int x = this.Location.X;
            int y = this.Location.Y;
            if (oldSize.Width != this.Width)
                x = x - this.Width / 2 + oldSize.Width / 2;
            if (oldSize.Height != this.Height)
                y = y - this.Height / 2 + oldSize.Height / 2;
            this.Location = new Point(x, y);
        }

    }
}
