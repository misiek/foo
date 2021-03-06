using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class DialogPanel : Panel
    {
        protected PictureBox closeButton;
        protected Label titleBar;
        public Label TitleBar
        {
            get
            {
                return this.titleBar;
            }
        }

        protected Panel content;
        public Panel Content
        {
            get
            {
                return this.content;
            }
        }


        public DialogPanel()
        {
            initialize();
        }

        private void initialize()
        {
            // close button
            Image closeImg = global::TouristGuide.Properties.Resources.close_small;
            closeButton = new TransparentPictureBox();
            closeButton.Size = closeImg.Size;
            closeButton.Image = closeImg;
            closeButton.BackColor = Color.YellowGreen;
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

            // content panel
            content = new Panel();
            content.Width = this.Width;
            content.Height = this.Height - closeButton.Height;
            content.Location = new Point(0, closeButton.Height);
            content.BackColor = Color.Yellow;
            Controls.Add(content);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            closeButton.Location = new Point(this.Width - closeButton.Width, 0);
            titleBar.Width = this.Width;
            content.Width = this.Width;
            content.Height = this.Height - closeButton.Height;
        }

        protected virtual void closeAction(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }

    }
}
