using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Gps
{
    public class GpsDevice
    {
        public event LocationChangedEventHandler locationChanged;
        public delegate void LocationChangedEventHandler(GpsDevice gps);
        public event SatellitesChangedEventHandler satellitesChanged;
        public delegate void SatellitesChangedEventHandler(GpsDevice gps);

        private SerialPort gpsPort;
        protected Thread gpsListenerThread;
        protected NmeaParser nmea;

        public GpsDevice()
        {
            this.gpsPort = new SerialPort();
            this.nmea = new NmeaParser();
        }

        public LocationData getLocationData()
        {
            return nmea.getLocationData();
        }

        public virtual void Open()
        {
            FindPort();
            CreateGpsListenerThread();
        }

        private void FindPort()
        {
            gpsPort.ReadTimeout = 300;
            Regex rxGps = new Regex("^[$]GP");
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    gpsPort.PortName = "COM" + i;
                    gpsPort.Open();
                    for (int j = 0; j < 3; j++)
                    {
                        try
                        {
                            string data = gpsPort.ReadLine();
                            if (gpsPort.IsOpen && rxGps.IsMatch(data))
                            {
                                Debug.WriteLine(gpsPort.PortName + ": data: " + data, this.ToString());
                                Debug.WriteLine(gpsPort.PortName + ": connected to gps device.", this.ToString());
                                return;
                            }
                        }
                        catch (TimeoutException ex)
                        {
                            //Debug.WriteLine(gpsPort.PortName + ": " + ex.Message, this.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(gpsPort.PortName + ": " + ex.Message, this.ToString());
                }
                gpsPort.Close();
            }
            Debug.WriteLine("couldn't connect to Gps device! " + gpsPort.PortName, this.ToString());
            return;
        }

        protected void CreateGpsListenerThread()
        {
            // we only want to create the thread if we don't have one created already 
            // and we have opened the gps device
            if (gpsListenerThread == null)
            {
                // Create and start thread to listen for GPS events
                gpsListenerThread = new System.Threading.Thread(new System.Threading.ThreadStart(Listen));
                gpsListenerThread.Start();
            }
        }

        protected void parseGpsMessage(string gpsMessage)
        {
            short status = nmea.Parse(gpsMessage);
            switch (status)
            {
                case NmeaParser.LOCATION:
                    if (locationChanged != null)
                    {
                        locationChanged(this);
                    }
                    break;
                case NmeaParser.SATELLITE:
                    if (satellitesChanged != null)
                    {
                        satellitesChanged(this);
                    }
                    break;
                case NmeaParser.UNRECOGNIZED:
                    break;
                case NmeaParser.CHECKSUM_INVALID:
                    break;
            }
        }

        protected virtual void Listen()
        {
            gpsPort.ReadTimeout = 50;
            lock (this)
            {
                bool listening = true;
                while (listening)
                {
                    string gpsMessage = "";
                    try
                    {
                        gpsMessage = gpsPort.ReadLine();
                        parseGpsMessage(gpsMessage);
                    }
                    catch (TimeoutException ex)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Debug.WriteLine(gpsPort.PortName + ": device disconnected?", this.ToString());
                        // TODO
                        // this.Close();
                        // this.Open(); ?
                    }
                }
            }
        } 

    }
}
