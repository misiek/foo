using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gps;

using OpenNETCF.GDIPlus;
//using OpenNETCF.Runtime.InteropServices.ComTypes;

namespace TouristGuide.gui
{
    public partial class MainWindow : Form
    {
        private GpsDevice gps;
        private EventHandler updatePosHandler;
        private EventHandler updateSatHandler;
        // current location
        private GpsLocation currentGpsLocation;

        // needed by gdiplus
        GdiplusStartupInput input = new GdiplusStartupInput();
        GdiplusStartupOutput output;
        IntPtr token;

        public MapPanel MapPanel
        {
            get
            {
                return this.mapPanel;
            }
        }

        public Label MapMessageBox
        {
            get
            {
                return this.mapMessageBox;
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
            // initialize gdiplus
            GpStatusPlus stat = NativeMethods.GdiplusStartup(out token, input, out output);
        }

        public void locationChanged(GpsLocation gpsLoc)
        {
            this.currentGpsLocation = gpsLoc;
            Invoke(this.updatePosHandler);
        }

        private void updateLocation(object sender, EventArgs args)
        {
            //GpsLocation gpsLoc = this.gps.getLocationData();
            if (this.panelCoordinates.Visible)
            {
                labelPosition.Text = this.currentGpsLocation.getLatitudeString() + " " + this.currentGpsLocation.getLongitudeString();
                labelSpeed.Text = this.currentGpsLocation.getSpeedKMH().ToString() + " km/h";
            }
        }

        public void satellitesChanged(GpsSatelites gpsSatelites)
        {
            //Invoke(this.updateSatHandler);
        }

        private void updateSatellite(object sender, EventArgs args)
        {
            //labelPosition.Text = "SATELLITE";
        }

        private void menuStartDevice_Click(object sender, EventArgs e)
        {
            this.menuStartSymulator.Checked = false;
            this.menuStartDevice.Checked = true;

            if (this.gps != null)
                this.gps.stop();
            this.gps = AppContext.Instance.getGpsDevice();
            restartGps();
        }

        private void menuStartSymulator_Click(object sender, EventArgs e)
        {
            this.menuStartDevice.Checked = false;
            this.menuStartSymulator.Checked = true;

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

        // download POIs
        private void menuItem2_Click(object sender, EventArgs e)
        {
            AppContext.Instance.getMapManager().downloadPois();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            // show poi browser
            AppContext.Instance.getPoiBrowser().display();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            this.menuItem7.Checked = !this.menuItem7.Checked;
            this.panelCoordinates.Visible = !this.panelCoordinates.Visible;
        }


    }
}