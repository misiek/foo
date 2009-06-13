using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class DialogPanel : Panel
    {
        protected Label titleBar;
        private PictureBox closeButton;


        public DialogPanel()
        {
            initialize();
        }

        private void initialize()
        {
            // close button
            Image closeImg = global::TouristGuide.Properties.Resources.close;
            closeButton = new PictureBox();
            closeButton.Size = closeImg.Size;
            closeButton.Image = closeImg;
            closeButton.Location = new Point(this.Width - closeButton.Width, 0);
            closeButton.Click += new EventHandler(closeAction);
            Controls.Add(closeButton);

            // title bar
            titleBar = new Label();
            titleBar.Width = this.Width;
            titleBar.Height = closeButton.Height;
            titleBar.Text = "DialogPanel";
            titleBar.BackColor = Color.YellowGreen;
            Controls.Add(titleBar);

            // this panel
            BackColor = Color.Yellow;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            closeButton.Location = new Point(this.Width - closeButton.Width, 0);
            titleBar.Width = this.Width;
        }

        private void closeAction(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

    }
}
