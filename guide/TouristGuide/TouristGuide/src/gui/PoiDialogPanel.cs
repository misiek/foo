using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class PoiDialogPanel : DialogPanel
    {
        private static int MARGIN = 3;

        private Poi poi;

        private TextBox descr;
        private LinkLabel moreLink;

        public PoiDialogPanel(Poi poi)
        {
            this.poi = poi;
            initialize();
        }

        private void initialize()
        {
            content.AutoScroll = false;

            // title
            this.titleBar.Text = poi.getName();

            int moreLinkHeight = 15;

            // description
            this.descr = new TextBox();
            this.descr.AcceptsReturn = true;
            this.descr.AcceptsTab = true;
            this.descr.Multiline = true;
            this.descr.ReadOnly = true;
            this.descr.ScrollBars = ScrollBars.Vertical;
            this.descr.Location = new Point(MARGIN, MARGIN);
            this.descr.Width = this.Width - 2 * MARGIN;
            this.descr.Height = this.Height - this.closeButton.Height - 3 * MARGIN - moreLinkHeight;
            this.descr.Text = poi.getDescr();
            this.content.Controls.Add(this.descr);

            this.moreLink = new LinkLabel();
            this.moreLink.Text = "more";
            this.moreLink.Height = moreLinkHeight;
            this.moreLink.Width = 50;
            this.moreLink.Location = new Point(MARGIN, 2 * MARGIN + this.descr.Height);
            this.moreLink.Click += new EventHandler(gotoPoiBrowser);
            this.content.Controls.Add(this.moreLink);
        }

        private void gotoPoiBrowser(object sender, EventArgs e)
        {
            AppContext.Instance.getPoiBrowser().preview(this.poi);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.descr.Width = this.Width - 2 * MARGIN;
            this.descr.Height = this.Height - this.closeButton.Height - 3 * MARGIN - this.moreLink.Height;
            this.moreLink.Location = new Point(MARGIN, 2 * MARGIN + this.descr.Height);
        }

    }
}
