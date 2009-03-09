using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gps;

namespace TouristGuide
{
    public partial class MainWindow : Form
    {
        private GpsDevice gps;
        private EventHandler updatePosHandler;
        private EventHandler updateSatHandler;
        // current location
        private GpsLocation currentGpsLocation;

        public Panel MapPanel
        {
            get
            {
                return this.mapPanel;
            }
        }

        public MainWindow()
        {
            this.updatePosHandler = new EventHandler(updateLocation);
            this.updateSatHandler = new EventHandler(updateSatellite);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void locationChanged(GpsLocation gpsLoc)
        {
            this.currentGpsLocation = gpsLoc;
            Invoke(this.updatePosHandler);
        }

        private void updateLocation(object sender, EventArgs args)
        {
            //GpsLocation gpsLoc = this.gps.getLocationData();
            labelPosition.Text = this.currentGpsLocation.getLatitudeString() + " " + this.currentGpsLocation.getLongitudeString();
            labelSpeed.Text = this.currentGpsLocation.getSpeed().ToString();
        }

        public void satellitesChanged()
        {
            Invoke(this.updateSatHandler);
        }

        private void updateSatellite(object sender, EventArgs args)
        {
            //labelPosition.Text = "SATELLITE";
        }

        private void menuStartDevice_Click(object sender, EventArgs e)
        {
            if (this.gps != null)
                this.gps.stop();
            this.gps = AppContext.Instance.getGpsDevice();
            restartGps();
        }

        private void menuStartSymulator_Click(object sender, EventArgs e)
        {
            if (this.gps != null)
                this.gps.stop();
            this.gps = AppContext.Instance.getGpsSymulator();
            restartGps();
        }

        private void restartGps()
        {
            // add gps events
            AppContext.Instance.getAppEvents().subscribeToGps(this.gps);
            // restart gps
            if (this.gps.isStarted())
                this.gps.stop();
            this.gps.start();
        }

        private void labelSpeed_ParentChanged(object sender, EventArgs e)
        {

        }


    }
}