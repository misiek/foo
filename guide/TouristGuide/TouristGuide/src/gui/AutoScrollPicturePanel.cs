using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TouristGuide.gui
{
    class AutoScrollPicturePanel : Panel
    {
        private PictureBox pictureBox;

        public AutoScrollPicturePanel()
        {
            initialize();
        }

        private void initialize()
        {
            AutoScroll = true;

            // picture box
            pictureBox = new PictureBox();
            Controls.Add(pictureBox);
        }

        public Image Image
        {
            set
            {
                this.pictureBox.Image = value;
                this.pictureBox.Size = value.Size;
            }
            get
            {
                return this.pictureBox.Image;
            }
        }

        public PictureBox PictureBox
        {
            get
            {
                return this.pictureBox;
            }
        }

    }
}
