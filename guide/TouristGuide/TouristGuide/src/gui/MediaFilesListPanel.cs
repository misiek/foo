using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TouristGuide.map.obj;
using System.Drawing;

namespace TouristGuide.gui
{
    class MediaFilesListPanel : Panel
    {
        private static int MARGIN = 3;

        private List<MediaFile> mediaFiles;
        private List<LinkLabel> mediaFilesLabels;
        private Control dialogParent;

        public MediaFilesListPanel(List<MediaFile> mediaFiles, Control dialogParent)
        {
            this.mediaFiles = mediaFiles;
            this.dialogParent = dialogParent;
            initialize();
        }

        private void initialize()
        {
            this.mediaFilesLabels = new List<LinkLabel>();
            int y = 0;
            foreach (MediaFile mf in this.mediaFiles)
            {
                LinkLabel ll = new MediaFileLinkLabel(mf, this.dialogParent);
                ll.Text = mf.getTitle();
                ll.Width = this.Width - 2 * MARGIN;
                ll.Location = new Point(MARGIN, y);
                this.mediaFilesLabels.Add(ll);
                this.Controls.Add(ll);
                y += ll.Height;
            }
            this.Height = y;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            foreach (LinkLabel ll in this.mediaFilesLabels)
            {
                ll.Width = this.Width - 2 * MARGIN;
            }
        }

    }
}
