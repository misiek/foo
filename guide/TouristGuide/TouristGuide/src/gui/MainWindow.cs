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
        private GpsDevice.LocationChangedEventHandler locationChangedHandler;
        private GpsDevice.SatellitesChangedEventHandler satellitesChangedHandler;
        private AppContext appContext;

        public MainWindow()
        {
            this.updatePosHandler = new EventHandler(updateLocation);
            this.updateSatHandler = new EventHandler(updateSatellite);

            this.locationChangedHandler = new GpsDevice.LocationChangedEventHandler(location);
            this.satellitesChangedHandler = new GpsDevice.SatellitesChangedEventHandler(satellite);

            InitializeComponent();
            this.appContext = AppContext.Instance;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void location()
        {
            Invoke(this.updatePosHandler);
        }

        private void satellite()
        {
            Invoke(this.updateSatHandler);
        }

        private void updateLocation(object sender, System.EventArgs args)
        {
            GpsLocation ld = this.gps.getLocationData();
            labelPosition.Text = ld.getLatitudeString() + " " + ld.getLongitudeString();
            labelSpeed.Text = ld.getSpeed().ToString();
        }

        private void updateSatellite(object sender, System.EventArgs args)
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
            // make sure that events are added once
            this.gps.locationChanged -= this.locationChangedHandler;
            this.gps.locationChanged += this.locationChangedHandler;
            this.gps.satellitesChanged -= this.satellitesChangedHandler;
            this.gps.satellitesChanged += this.satellitesChangedHandler;
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