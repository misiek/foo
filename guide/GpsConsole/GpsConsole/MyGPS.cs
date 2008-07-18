using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.IO.Ports;

using System.Diagnostics;

namespace GpsConsole
{
    class MyGPS
    {
        private SerialPort gpsPort = new SerialPort();


        public MyGPS()
        {
        }

        public void Open()
        {
            findPort();
            
        }

        private void findPort()
        {
            gpsPort.ReadTimeout = 200;
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
            return;
        }

        public void read()
        {
            if (gpsPort.IsOpen)
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
                            Console.WriteLine("gpsMessage: " + gpsMessage);
                        }
                        catch (TimeoutException ex)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        
                    }
                }
            }
            else
            {
                Debug.WriteLine("couldn't connect to Gps device! " + gpsPort.PortName, this.ToString());
            }

        } 

    }
}
