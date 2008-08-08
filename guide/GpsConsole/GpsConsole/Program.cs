using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GpsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //IntPtr location = new IntPtr(1);
            //IntPtr satellite = new IntPtr(1);
            //GpsDevice gps = new GpsDevice(satellite, location);
            GpsDevice gps = new GpsDevice();
            gps.Open();


            Listener l = new Listener();
            l.SubscribeLocation(gps);
            l.SubscribeSatellites(gps);
        }

    }


    public class Listener
    {
        public void SubscribeLocation(GpsDevice gps)
        {
            gps.locationChanged += new GpsDevice.LocationChangedEventHandler(location);
        }

        public void SubscribeSatellites(GpsDevice gps)
        {
            gps.satellitesChanged += new GpsDevice.SatellitesChangedEventHandler(satellite);
        }

        private void location(GpsDevice gps)
        {
            System.Console.WriteLine("LOCATION");
        }

        private void satellite(GpsDevice gps)
        {
            System.Console.WriteLine("SATELLITE");
        }
    }

}
