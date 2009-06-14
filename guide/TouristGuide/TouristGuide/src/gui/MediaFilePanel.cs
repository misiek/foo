using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TouristGuide.map.obj;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace TouristGuide.gui
{
    class MediaFilePanel : DialogPanel
    {
        private static int MARGIN = 3;

        private TextBox descr;
        private AutoScrollPicturePanel imgPanel;
        

        private int y = 0;

        private MediaFile media;

        public MediaFilePanel(MediaFile media)
        {
            this.media = media;
            initialize();
        }

        private void initialize()
        {
            // title
            this.titleBar.Text = this.media.getTitle();

            // description
            this.y += MARGIN;
            this.descr = new TextBox();
            this.descr.AcceptsReturn = true;
            this.descr.AcceptsTab = true;
            this.descr.Multiline = true;
            this.descr.ReadOnly = true;
            this.descr.ScrollBars = ScrollBars.Vertical;
            this.descr.Location = new Point(MARGIN, y);
            this.descr.Width = this.Width - 2 * MARGIN;
            this.descr.Height = 2 * this.titleBar.Height;
            this.descr.Text = this.media.getDescr();
            this.content.Controls.Add(this.descr);

            // picture
            this.imgPanel = new AutoScrollPicturePanel();
            this.y += this.descr.Height + MARGIN;
            this.imgPanel.Location = new Point(MARGIN, y);
            this.imgPanel.Width = this.Width - 2 * MARGIN;
            this.imgPanel.Height = this.content.Height - y - MARGIN;
            this.content.Controls.Add(this.imgPanel);

            this.imgPanel.PictureBox.Click += new EventHandler(fullScreen);
        }

        public void loadImage()
        {
            Bitmap mediaBmp = new Bitmap(new MemoryStream(this.media.getMedia()));
            this.imgPanel.Image = mediaBmp;
        }

        private void fullScreen(object sender, EventArgs e)
        {
            Debug.WriteLine(" *** CLICK fullScreen *** ", ToString());
        }

        protected override void closeAction(object sender, EventArgs e)
        {
            base.closeAction(sender, e);
            this.imgPanel.Image = null;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.descr.Width = this.Width - 2 * MARGIN;
            this.imgPanel.Width = this.Width - 2 * MARGIN;
            this.imgPanel.Height = this.content.Height - y - MARGIN;
        }

    }
}
