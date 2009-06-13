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

        private Control dialogParent;

        public MediaFileLinkLabel(MediaFile media, Control dialogParent)
        {
            this.media = media;
            this.dialogParent = dialogParent;
            this.mediaPanel = new MediaFilePanel(media);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.mediaPanel.Location = new Point();
            this.mediaPanel.Size = this.dialogParent.Size;
            this.dialogParent.Controls.Add(this.mediaPanel);
            this.mediaPanel.BringToFront();
        }

    }
}
