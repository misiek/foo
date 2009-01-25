using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Gps
{
    public class GpsSymulator : GpsDevice
    {
        private string gpsLogDirPath;

        public GpsSymulator(string dirPath)
        {
            this.gpsLogDirPath = dirPath + "\\gpslog";
            this.nmea = new NmeaParser();
        }

        override public void start()
        {
            Debug.WriteLine("GpsSymulator: start");
            this.listening = true;
            createGpsListenerThread();
        }

        /*
         * Read gps messages from log files.
         */
        override protected void listen()
        {
            Debug.WriteLine("GpsSymulator: listen");
            lock (this)
            {
                DirectoryInfo gpsLogDirInfo = new DirectoryInfo(this.gpsLogDirPath);
                foreach (FileInfo gpslogFileInfo in gpsLogDirInfo.GetFiles("*.gps"))
                {
                    if (!this.listening)
                        break;
                    Debug.WriteLine("GpsSymulator: Listen: reading gps log: " + gpslogFileInfo.ToString());
                    string pathToGpsLog = this.gpsLogDirPath + "\\" + gpslogFileInfo.ToString();
                    // create reader & open file
                    TextReader gpsLogTr = new StreamReader(pathToGpsLog);
                    string gpsMessage;
                    while ((gpsMessage = gpsLogTr.ReadLine()) != null && this.listening)
                    {
                        parseGpsMessage(gpsMessage);
                        System.Threading.Thread.Sleep(200);
                    }
                    // close the stream
                    gpsLogTr.Close();
                }
                Debug.WriteLine("listen: finished reading gps logs.", this.ToString());
            }
        }
    }
}
