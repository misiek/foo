﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GpsGuide.Gps;

namespace GpsGuide
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Label labelPosition;
        GpsDevice gps = new GpsDevice();
        private EventHandler updatePosHandler;
        private EventHandler updateSatHandler;
            

        public Form1()
        {
            InitializeComponent();
            gps.Open();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gps.locationChanged     += new GpsDevice.LocationChangedEventHandler(location);
            //gps.satellitesChanged   += new GpsDevice.SatellitesChangedEventHandler(satellite);

            updatePosHandler = new EventHandler(updateLocation);
            //updateSatHandler = new EventHandler(updateSatellite);
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
            LocationData ld = gps.getLocationData();
            labelPosition.Text = ld.getLatitudeString() + " " + ld.getLongitudeString();
            labelSpeed.Text = ld.getSpeed();
        }

        private void updateSatellite(object sender, System.EventArgs args)
        {
            //labelPosition.Text = "SATELLITE";
        }

    }
}