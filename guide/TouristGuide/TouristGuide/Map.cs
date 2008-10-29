using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gps;

namespace TouristGuide
{
    class Map
    {
        private Panel mapContainer;
        private GpsDevice gps;

        public Map(Panel mapContainer, GpsDevice gps)
        {
            this.mapContainer = mapContainer;
            this.gps = gps;
        }

        /* 
         * PictureBox pictureBox1;
         * pictureBox1 = new System.Windows.Forms.PictureBox();
         * pictureBox1.BackColor = System.Drawing.Color.FromArgb(
         *      ((int)(((byte)(192)))), 
         *      ((int)(((byte)(192)))), 
         *      ((int)(((byte)(255)))));
         * pictureBox1.Location = new System.Drawing.Point(0, 0);
         * pictureBox1.Name = "pictureBox1";
         * pictureBox1.Size = new System.Drawing.Size(125, 84);
         * pictureBox1.Parent = mapContainer;
         * mapContainer.Controls.Add(pictureBox1);
         * 
         */
    }
}
