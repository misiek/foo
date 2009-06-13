using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class MainDetailLinkLabel : LinkLabel
    {
        private MainDetail detail;
        private MainDetailPanel mainDetailPanel;

        private Form poiBrowser;

        public MainDetailLinkLabel(MainDetail detail, Form poiBrowser)
        {
            this.detail = detail;
            this.poiBrowser = poiBrowser;
            this.mainDetailPanel = new MainDetailPanel(detail);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.mainDetailPanel.Location = new Point();
            this.mainDetailPanel.Size = this.poiBrowser.Size;
            this.poiBrowser.Controls.Add(this.mainDetailPanel);
            this.mainDetailPanel.BringToFront();
        }
    }
}
