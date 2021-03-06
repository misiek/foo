using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using TouristGuide.map.obj;
using TouristGuide.gui;
using TouristGuide.util;

namespace TouristGuide.map
{
    public class MapDisplayer
    {
        private EventHandler updateMapPanelHandler;
        private Point[] orderingPoints;
        private MapView mapView;
        private MapPanel mapPanel;

        private EventHandler updateMapMessageBoxHandler;
        private EventHandler hideMapMessageBoxHandler;
        private bool cleanPoiLabelsHandler = false;
        private string message;
        private System.Threading.Timer hideMessageTimer;
        private string timerMessage;
        private Label mapMessageBox;
        private Color mapMessageBoxDefaultColor;
        private Color mapMessageBoxColor;

        private Hashtable poiLabels = new Hashtable();

        private Point currentCenterPoint;

        private Poi currentTarget;

        public MapDisplayer(MapPanel mapPanel, Label mapMessageBox)
        {
            this.mapPanel = mapPanel;
            this.mapMessageBox = mapMessageBox;
            this.mapMessageBoxDefaultColor = this.mapMessageBox.BackColor;
            this.mapMessageBoxColor = this.mapMessageBoxDefaultColor;
            this.updateMapPanelHandler = new EventHandler(updateMapPanel);
            this.updateMapMessageBoxHandler = new EventHandler(updateMapMessageBox);
            this.hideMapMessageBoxHandler = new EventHandler(hideMapMessageBox);
            this.message = "";
            this.timerMessage = "";
            this.poiLabels = new Hashtable();
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
            if (mapView == null)
            {
                Debug.WriteLine("displayView: error: mapView null!", ToString());
            }
            // set ordering points for display process
            this.orderingPoints = mapView.getOrderingPoints();
            // set map view to dispaly
            if (this.mapView != mapView)
            {
                this.mapView = mapView;
                // set flag to clean labels cache in updateMapPanelHandler thread
                this.cleanPoiLabelsHandler = true;

            }
            // invoke gui panel update in its thread
            this.mapPanel.Invoke(updateMapPanelHandler);
            //// test
            //displayMessage("New map view: " + mapView.getGpsLocation().getLatitude()
            //               + ", " + mapView.getGpsLocation().getLongitude());
        }

        private void updateMapPanel(object sender, EventArgs args)
        {
            this.currentCenterPoint = new Point(this.mapPanel.Width / 2, this.mapPanel.Height / 2);
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
                    Point centerImgLocation = this.mapView.getCenterImgPosition();
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
                    pSlot.Position = new Point(this.currentCenterPoint.X + tWidth - centerImgLocation.X,
                                              this.currentCenterPoint.Y + tHeight - centerImgLocation.Y);
                }
                // when there is no map image empty slot
                else
                {
                    pSlot.Image = null;
                }
            }
            
            // dispaly line to current target, and message when target changed
            updateCurrentTarget();
            // set rotation to course
            rotateToCourse();
            
            mapPanel.update();

            displayPois();

            // refresh map panel
            mapPanel.Refresh();
        }

        private void rotateToCourse()
        {
            //double course = this.mapView.getGpsLocation().getCourse();
            double course = this.mapView.getEstimatedCourse();
            //Debug.WriteLine("rotateToCourse: COURSE: " + course, this.ToString());
            this.mapPanel.setRotation(course);

        }

        private void updateCurrentTarget()
        {
            if (this.mapView.getTarget() == null)
            {
                this.mapPanel.DirectionLineVisible = false;
                return;
            }
            if (this.currentTarget != this.mapView.getTarget()) {
                displayMessage("Next target: " + this.mapView.getTarget(), 5000);
                this.currentTarget = this.mapView.getTarget();
            }
            Point targetPoint = this.mapView.getTargetPixelCoordinates();
            Point panelTargetPoint = getMapPanelCoordinates(targetPoint);
            Debug.WriteLine("updateMapPanel: PANEL TARGET POINT: "
                + PointUtil.pointStr(panelTargetPoint), ToString());
            // show line to current target
            this.mapPanel.showDirectionLine(panelTargetPoint);
        }

        private void displayPois()
        {
            if (this.cleanPoiLabelsHandler)
                cleanPoiLabels();

            if (this.mapView.getPois() == null)
            {
                return;
            }
            foreach (Poi poi in this.mapView.getPois())
            {
                Control pLabel;
                // TODO: consider removing from labels cache
                if (!poiLabels.ContainsKey(poi.ToString()))
                {
                    // TODO: Add poi gui element class
                    pLabel = new PoiPanel(poi, mapPanel);
                    poiLabels[poi.ToString()] = pLabel;
                    this.mapPanel.Controls.Add(pLabel);
                    //Debug.WriteLine("displayPois: added label for: @@@@@ " + poi.getName(), ToString());
                }
                else
                {
                    pLabel = (Control)poiLabels[poi.ToString()];
                }
                Point poiMapViewPosition = this.mapView.getPoiPixelCoordinates(poi);
                updateLabelLocation(pLabel, poiMapViewPosition);
            }
        }

        private Point getMapPanelCoordinates(Point p)
        {
            Point mapViewPositionOnImg = this.mapView.getPositionOnImg();
            int xMapViewTr = mapViewPositionOnImg.X - this.currentCenterPoint.X;
            int yMapViewTr = mapViewPositionOnImg.Y - this.currentCenterPoint.Y;
            return new Point(p.X - xMapViewTr, p.Y - yMapViewTr);
        }

        private void updateLabelLocation(Control l, Point poiMapViewPosition)
        {
            Point mapPanelCoordinates = getMapPanelCoordinates(poiMapViewPosition);

            int xSizeFix = l.Size.Width / 2;
            int ySizeFix = l.Size.Height / 2;

            l.Location = new Point(mapPanelCoordinates.X - xSizeFix,
                                   mapPanelCoordinates.Y - ySizeFix);

            //l.Location = new Point(mapPanelCoordinates.X,
            //                       mapPanelCoordinates.Y);
        }

        private void cleanPoiLabels()
        {
            // TODO rewrite old suitable labels
            foreach (Control c in this.poiLabels.Values)
            {
                this.mapPanel.Controls.Remove(c);
            }
            this.poiLabels = new Hashtable();
            this.cleanPoiLabelsHandler = false;
        }

    }
}
