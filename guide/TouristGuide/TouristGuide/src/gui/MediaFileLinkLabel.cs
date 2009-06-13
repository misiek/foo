using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TouristGuide.map.obj;
using System.Drawing;

namespace TouristGuide.gui
{
    class MediaFileLinkLabel : LinkLabel
    {
        private MediaFile media;
        private MediaFilePanel mediaPanel;

        private Form poiBrowser;

        public MediaFileLinkLabel(MediaFile media, Form poiBrowser)
        {
            this.media = media;
            this.poiBrowser = poiBrowser;
            this.mediaPanel = new MediaFilePanel(media);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.mediaPanel.Location = new Point();
            this.mediaPanel.Size = this.poiBrowser.Size;
            this.poiBrowser.Controls.Add(this.mediaPanel);
            this.mediaPanel.BringToFront();
        }

    }
}
