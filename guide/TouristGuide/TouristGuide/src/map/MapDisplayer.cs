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
        private EventHandler updateMapPanelHandler;
        private ArrayList orderingPoints;
        private MapView mapView;
        private Panel mapPanel;
        private Hashtable pictureBoxes;
        private Label positionMarker;

        public MapDisplayer(Panel mapPanel)
        {
            this.mapPanel = mapPanel;
            this.updateMapPanelHandler = new EventHandler(updateMapPanel);
            initializeMapPanel();
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
            // set ordering points for display process
            this.orderingPoints = mapView.getOrderingPoints();
            // set map view to dispaly
            this.mapView = mapView;
            // invoke gui panel update in its thread
            this.mapPanel.Invoke(updateMapPanelHandler);
        }

        private void updateMapPanel(object sender, EventArgs args)
        {
            Point centerPoint = new Point(this.mapPanel.Width / 2, this.mapPanel.Height / 2);
            this.positionMarker.Location = new Point(centerPoint.X - 2, centerPoint.Y - 2);
            this.positionMarker.Visible = true;
            foreach (Point p in this.orderingPoints)
            {
                Image mapPartImg = this.mapView.getImgByPoint(p);
                PictureBox pBox = (PictureBox)this.pictureBoxes[p];
                if (mapPartImg != null)
                {
                    // center image's coordinates (1, 1) 
                    // so translation vector is:
                    int tx = p.X - 1;
                    int ty = p.Y - 1;
                    if (pBox.Image != mapPartImg)
                    {
                        pBox.Image = mapPartImg;
                        pBox.Size = new Size(mapPartImg.Width, mapPartImg.Height);
                    }
                    // location of image from point and img size,
                    // center image in (0, 0) of map panel
                    Point imgLocation = new Point(centerPoint.X + tx * mapPartImg.Width,
                                                  centerPoint.Y + ty * mapPartImg.Height);
                    // translate all images according to map location in center image
                    Point centerImgLocation = this.mapView.getCenterImgLocation();
                    imgLocation.X -= centerImgLocation.X;
                    imgLocation.Y -= centerImgLocation.Y;
                    pBox.Location = imgLocation;
                    pBox.Visible = true;
                }
                else
                {
                    pBox.Visible = false;
                }
                // refresh map panel
                this.mapPanel.Refresh();
            }
        }

        // Initialize map panel with picture boxes,
        // which are used as slots for parts of view to display.
        private void initializeMapPanel()
        {
            // initialize position marker
            this.positionMarker = new Label();
            this.positionMarker.BackColor = Color.Red;
            this.positionMarker.Size = new Size(5, 5);
            this.positionMarker.Visible = false;
            this.mapPanel.Controls.Add(this.positionMarker);
            // initialize picture boxes
            this.pictureBoxes = new Hashtable();
            this.pictureBoxes[new Point(1, 1)] = new PictureBox();
            this.pictureBoxes[new Point(0, 1)] = new PictureBox();
            this.pictureBoxes[new Point(0, 2)] = new PictureBox();
            this.pictureBoxes[new Point(1, 2)] = new PictureBox();
            this.pictureBoxes[new Point(2, 2)] = new PictureBox();
            this.pictureBoxes[new Point(2, 1)] = new PictureBox();
            this.pictureBoxes[new Point(2, 0)] = new PictureBox();
            this.pictureBoxes[new Point(1, 0)] = new PictureBox();
            this.pictureBoxes[new Point(0, 0)] = new PictureBox();
            // add picture boxes to map panel
            foreach (DictionaryEntry entry in this.pictureBoxes)
            {
                PictureBox pBox = (PictureBox)entry.Value;
                pBox.Name = "pictureBox" + ((Point)entry.Key).X + ((Point)entry.Key).Y;
                pBox.Visible = false;
                this.mapPanel.Controls.Add(pBox);
            }
        }
    }
}
