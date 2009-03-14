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

        private EventHandler updateMapMessageBoxHandler;
        private EventHandler hideMapMessageBoxHandler;
        private string message;
        private Label mapMessageBox;

        public MapDisplayer(Panel mapPanel)
        {
            this.mapPanel = mapPanel;
            this.updateMapPanelHandler = new EventHandler(updateMapPanel);
            this.updateMapMessageBoxHandler = new EventHandler(updateMapMessageBox);
            this.hideMapMessageBoxHandler = new EventHandler(hideMapMessageBox);
            initializeMapPanel();
        }

        // display message for specified time
        public void displayMessage(string message, int mSecounds)
        {

        }

        // display message in map pannel
        // message is shown until hideMessage() or next dispalayMessage
        public void displayMessage(string message)
        {
            this.message = message;
            this.mapPanel.Invoke(updateMapMessageBoxHandler);
        }

        // hide message but not time message
        public void hideMessage(string message)
        {
            if (this.message == message) // TODO: and time
                this.mapPanel.Invoke(hideMapMessageBoxHandler);
        }

        private void updateMapMessageBox(object sender, EventArgs args)
        {
            this.mapMessageBox.Text = this.message;
            this.mapMessageBox.Visible = true;
            this.mapMessageBox.Refresh();
        }

        private void hideMapMessageBox(object sender, EventArgs args)
        {
            this.mapMessageBox.Visible = false;
            this.mapMessageBox.Refresh();
        }

        public void displayView(MapView mapView)
        {
            
            // set ordering points for display process
            this.orderingPoints = mapView.getOrderingPoints();
            // set map view to dispaly
            this.mapView = mapView;
            // invoke gui panel update in its thread
            this.mapPanel.Invoke(updateMapPanelHandler);
            //// test
            //displayMessage("New map view: " + mapView.getGpsLocation().getLatitude()
            //               + ", " + mapView.getGpsLocation().getLongitude());
        }

        private void updateMapPanel(object sender, EventArgs args)
        {
            // create the center point of map panel
            Point centerPoint = new Point(this.mapPanel.Width / 2, this.mapPanel.Height / 2);
            // set location of the position marker to the center point
            this.positionMarker.Location = new Point(centerPoint.X - 2, centerPoint.Y - 2);
            // show position marker when its hidden
            if (!this.positionMarker.Visible)
                this.positionMarker.Visible = true;
            // iterate through view points
            PictureBox pBoxCenter = (PictureBox)this.pictureBoxes[new Point(1, 1)];
            foreach (Point p in this.orderingPoints)
            {
                // get map image for the point
                Image mapPartImg = this.mapView.getImgByPoint(p);
                // get picture box for the point
                PictureBox pBox = (PictureBox)this.pictureBoxes[p];
                if (mapPartImg != null)
                {
                    // update image only when picture box contains different one
                    if (pBox.Image != mapPartImg)
                    {
                        pBox.Image = mapPartImg;
                        pBox.Size = new Size(mapPartImg.Width, mapPartImg.Height);
                    }
                    // Map MOVEMENT translation translates whole map view to show gps location:
                    // get location in center image to translate all images according to this location
                    // this make movement of map
                    Point centerImgLocation = this.mapView.getCenterImgLocation();
                    // View PARTS translation places 9 map parts side by side in map panel:
                    // center image's coordinates (1, 1) 
                    // so translation vector is:
                    int tx = p.X - 1;
                    int ty = p.Y - 1;
                    // x translation: depending on direction of translation we use different width
                    int tWidth = 0;
                    // translation to left width of current image
                    if (tx < 0)
                        tWidth = tx * mapPartImg.Width;
                    // translation to right width of image before current
                    else if (tx > 0)
                        // NOTE: this is correct becouse only edge parts can have different sizes than others
                        tWidth = tx * pBoxCenter.Width;
                    // y translation: the same with as with x
                    int tHeight = 0;
                    if (ty < 0)
                        tHeight = ty * mapPartImg.Height;
                    else if (ty > 0)
                        // NOTE: this is correct becouse only edge parts can have different sizes than others
                        tHeight = ty * pBoxCenter.Height;
                    // Position of image is addition of movament and parts translation
                    pBox.Location = new Point(centerPoint.X + tWidth - centerImgLocation.X,
                                              centerPoint.Y + tHeight - centerImgLocation.Y);
                    // when picture box is hidden make it visible
                    if (!pBox.Visible)
                        pBox.Visible = true;
                }
                // when there is no map image hide picture box
                else
                {
                    pBox.Visible = false;
                }
            }
            // refresh map panel
            this.mapPanel.Refresh();
        }

        // Initialize map panel with picture boxes,
        // which are used as slots for parts of view to display.
        private void initializeMapPanel()
        {
            this.mapMessageBox = (Label)this.mapPanel.Controls[0];
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
