using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.IO.Ports;

using System.Diagnostics;

namespace GpsConsole
{
    class GpsDevice
    {
        private SerialPort gpsPort = new SerialPort();
        private System.Threading.Thread gpsListenerThread = null;

        private IntPtr stateChange;
        private IntPtr locationChange;

        private NmeaParser nmea = new NmeaParser();

        //public GpsDevice(IntPtr stateChange, IntPtr locationChange)
        public GpsDevice()
        {
            this.stateChange = stateChange;
            this.locationChange = locationChange;
        }

        public void Open()
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
                            Debug.WriteLine(gpsPort.PortName + ": " + ex.Message, this.ToString());
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

        private void CreateGpsListenerThread()
        {
            // we only want to create the thread if we don't have one created already 
            // and we have opened the gps device
            if (gpsListenerThread == null && gpsPort.IsOpen)
            {
                // Create and start thread to listen for GPS events
                gpsListenerThread = new System.Threading.Thread(new System.Threading.ThreadStart(Listen));
                gpsListenerThread.Start();
            }
        }

        private void Listen()
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
                        nmea.Parse(gpsMessage);
                    }
                    catch (TimeoutException ex)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    
                }
            }
        } 

    }
}
