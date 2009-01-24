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
        private AppContext appContext;

        public MainWindow()
        {
            InitializeComponent();
            this.appContext = AppContext.Instance;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void location(GpsDevice gps)
        {
            Invoke(updatePosHandler);
        }

        private void satellite(GpsDevice gps)
        {
            Invoke(updateSatHandler);
        }

        private void updateLocation(object sender, System.EventArgs args)
        {
            LocationData ld = this.gps.getLocationData();
            labelPosition.Text = ld.getLatitudeString() + " " + ld.getLongitudeString();
            labelSpeed.Text = ld.getSpeed().ToString();
        }

        private void updateSatellite(object sender, System.EventArgs args)
        {
            //labelPosition.Text = "SATELLITE";

           
        }

        private void menuStartDevice_Click(object sender, EventArgs e)
        {
            //gps.Open();
        }

        private void menuStartSymulator_Click(object sender, EventArgs e)
        {
            this.gps = AppContext.Instance.getGpsSymulator();
            this.gps.locationChanged += new GpsDevice.LocationChangedEventHandler(location);
            this.gps.satellitesChanged += new GpsDevice.SatellitesChangedEventHandler(satellite);

            updatePosHandler = new EventHandler(updateLocation);
            updateSatHandler = new EventHandler(updateSatellite);
            this.gps.Open();
        }

        private void labelSpeed_ParentChanged(object sender, EventArgs e)
        {

        }


    }
}