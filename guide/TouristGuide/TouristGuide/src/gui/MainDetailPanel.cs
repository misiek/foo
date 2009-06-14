using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class MainDetailPanel : DialogPanel
    {
        private static int MARGIN = 3;
        private static int MARGIN_RIGHT = 20;
        private static int HEIGHT_DESCR = 250;

        private MainDetail detail;
        private Control poiBrowser;
        private TextBox descr;
        private MediaFilesListPanel mediaFilesPanel;
        private int y = 0;

        public MainDetailPanel(MainDetail detail, Control poiBrowser)
        {
            this.detail = detail;
            this.poiBrowser = poiBrowser;
            initialize();
        }

        private void initialize()
        {
            this.content.AutoScroll = true;

            // title
            this.titleBar.Text = this.detail.getTitle();

            // description
            this.y += MARGIN;
            this.descr = new TextBox();
            this.descr.AcceptsReturn = true;
            this.descr.AcceptsTab = true;
            this.descr.Multiline = true;
            this.descr.ReadOnly = true;
            this.descr.ScrollBars = ScrollBars.Vertical;
            this.descr.Location = new Point(MARGIN, this.y);
            this.descr.Width = this.Width - MARGIN - MARGIN_RIGHT;
            //this.descr.Height = this.content.Height - 2 * MARGIN;
            this.descr.Height = HEIGHT_DESCR;
            this.descr.Text = this.detail.getDescr();
            this.content.Controls.Add(this.descr);

            // media files
            this.y += this.descr.Height + MARGIN;
            displayMediaFiles();
        }

        private void displayMediaFiles()
        {
            List<MediaFile> mediaFiles = this.detail.getMediaFiles();
            if (mediaFiles.Count > 0)
            {
                Label ll = new Label();
                ll.Text = "Media files:";
                ll.Location = new Point(MARGIN, this.y);
                this.content.Controls.Add(ll);
                this.y += ll.Height;
                this.mediaFilesPanel = new MediaFilesListPanel(mediaFiles, this.poiBrowser);
                this.mediaFilesPanel.Width = this.Width - MARGIN - MARGIN_RIGHT;
                this.mediaFilesPanel.Location = new Point(MARGIN, this.y);
                this.content.Controls.Add(this.mediaFilesPanel);
                this.y += this.mediaFilesPanel.Height;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.descr.Width = this.Width - MARGIN - MARGIN_RIGHT;
            if (this.mediaFilesPanel != null)
                this.mediaFilesPanel.Width = this.Width - MARGIN - MARGIN_RIGHT;
        }

    }
}
