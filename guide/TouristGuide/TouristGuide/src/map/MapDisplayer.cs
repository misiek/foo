using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

using TouristGuide.map.obj;
using TouristGuide.gui;

namespace TouristGuide.map
{
    public class MapDisplayer
    {
        private EventHandler updateMapPanelHandler;
        private ArrayList orderingPoints;
        private MapView mapView;
        private MapPanel mapPanel;

        private EventHandler updateMapMessageBoxHandler;
        private EventHandler hideMapMessageBoxHandler;
        private string message;
        private System.Threading.Timer hideMessageTimer;
        private string timerMessage;
        private Label mapMessageBox;
        private Color mapMessageBoxDefaultColor;
        private Color mapMessageBoxColor;

        private Hashtable poiLabels = new Hashtable();

        public MapDisplayer(MapPanel mapPanel)
        {
            this.mapPanel = mapPanel;
            this.mapMessageBox = (Label)this.mapPanel.Controls[0];
            this.mapMessageBoxDefaultColor = this.mapMessageBox.BackColor;
            this.mapMessageBoxColor = this.mapMessageBoxDefaultColor;
            this.updateMapPanelHandler = new EventHandler(updateMapPanel);
            this.updateMapMessageBoxHandler = new EventHandler(updateMapMessageBox);
            this.hideMapMessageBoxHandler = new EventHandler(hideMapMessageBox);
            this.message = "";
            this.timerMessage = "";
        }

        // display message for specified time
        public void displayMessage(string message, int mSecounds)
        {
            if (this.timerMessage=="")
            {
                this.timerMessage = message;
                this.hideMessageTimer = new System.Threading.Timer(
                                    new System.Threading.TimerCallback(hideMessageCallback),
                                    null, mSecounds, System.Threading.Timeout.Infinite);
                displayMessage(this.timerMessage);
            }
            // make time longer when timer is running and message is the same
            else if (this.timerMessage == message && this.hideMessageTimer!=null)
            {
                this.hideMessageTimer.Change(mSecounds, System.Threading.Timeout.Infinite);
            }
        }

        private void hideMessageCallback(object sender)
        {
            hideMessage(this.timerMessage);
            this.timerMessage = "";
            this.hideMessageTimer = null;
        }


        public void displayMessage(string message, int mSecounds, Color bgColor)
        {
            this.mapMessageBoxColor = bgColor;
            displayMessage(message, mSecounds);
        }

        // display message in map pannel
        // message is shown until hideMessage() or next dispalayMessage
        public void displayMessage(string message)
        {
            this.message = message;
            this.mapPanel.Invoke(updateMapMessageBoxHandler);
        }

        public void displayMessage(string message, Color bgColor)
        {
            this.mapMessageBoxColor = bgColor;
            displayMessage(message);
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
            this.mapMessageBox.BackColor = this.mapMessageBoxColor;
            this.mapMessageBox.Visible = true;
            this.mapMessageBox.Refresh();
        }

        private void hideMapMessageBox(object sender, EventArgs args)
        {
            this.mapMessageBox.Visible = false;
            this.mapMessageBox.BackColor = this.mapMessageBoxDefaultColor;
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
            Point centerPoint = new Point(this.mapPanel.Width / 2, this.mapPanel.Height / 2);
            PictureSlot pSlotCenter = (PictureSlot)this.mapPanel.PictureSlots[new Point(1, 1)];
            foreach (Point p in this.orderingPoints)
            {
                // get map image for the point
                Bitmap mapPartImg = (Bitmap)this.mapView.getImgByPoint(p);
                // get picture slot for the point
                PictureSlot pSlot = (PictureSlot)this.mapPanel.PictureSlots[p];
                if (mapPartImg != null)
                {
                    // update image only when picture slot contains different one
                    if (pSlot.Image != mapPartImg)
                    {
                        pSlot.Image = mapPartImg;
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
                        tWidth = tx * pSlotCenter.Image.Width;
                    // y translation: the same with as with x
                    int tHeight = 0;
                    if (ty < 0)
                        tHeight = ty * mapPartImg.Height;
                    else if (ty > 0)
                        // NOTE: this is correct becouse only edge parts can have different sizes than others
                        tHeight = ty * pSlotCenter.Image.Height;
                    // Position of image is addition of movament and parts translation
                    pSlot.Position = new Point(centerPoint.X + tWidth - centerImgLocation.X,
                                              centerPoint.Y + tHeight - centerImgLocation.Y);
                }
                // when there is no map image empty slot
                else
                {
                    pSlot.Image = null;
                }
            }
            // just for test
            //this.mapPanel.showDirectionLine(new Point(50, 50));
            // refresh map panel
            //this.mapPanel.Refresh();

            foreach (Poi poi in mapView.getPois())
            {
             
                if (!poiLabels.ContainsKey(poi.ToString()))
                {
                    // TODO: Add poi gui element class
                    Label newLabel = new Label();
                    poiLabels[poi.ToString()] = newLabel;
                    this.mapPanel.Controls.Add(newLabel);
                }
                Label l = (Label)poiLabels[poi.ToString()];

                Point poiMapViewPosition = mapView.getPoiPixelCoordinates(poi);

                l.Location = new Point(poiMapViewPosition.X, poiMapViewPosition.Y);
                l.Text = poi.getName();
                

            }

            this.mapPanel.Refresh();
        }

    }
}
