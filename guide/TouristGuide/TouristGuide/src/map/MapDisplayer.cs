using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

using TouristGuide.map.obj;

namespace TouristGuide.map
{
    public class MapDisplayer
    {
        EventHandler updateMapPanelHandler;

        private Panel mapPanel;
        public Panel MapPanel
        {
            get
            {
                return this.mapPanel;
            }
            set
            {
                this.mapPanel = value;
            }
        }

        // only for tests
        Image img;

        public MapDisplayer()
        {
            this.updateMapPanelHandler = new EventHandler(updateMapPanel);
        }

        /// <summary>
        /// Displays loading message in map panel.
        /// </summary>
        public void loadingMap()
        {
            throw new System.NotImplementedException();
        }

        public void displayView(MapView mapView)
        {
            Debug.WriteLine("new MapView to display", this.ToString());

            ArrayList orderingPoints = mapView.getOrderingPoints();
            this.img = mapView.getImgByPoint((Point)orderingPoints[0]);

            

            this.mapPanel.Invoke(updateMapPanelHandler);
        }

        private void updateMapPanel(object sender, EventArgs args)
        {
            // just for tests
            PictureBox pictureBox1 = new PictureBox();
            pictureBox1.Image = this.img;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(266, 279);
            mapPanel.Controls.Add(pictureBox1);
            mapPanel.Refresh();
        }
    }
}
