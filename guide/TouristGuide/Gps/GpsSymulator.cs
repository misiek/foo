using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

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

        override public void Open()
        {
            Debug.WriteLine("GpsSymulator: Open");
            CreateGpsListenerThread();
        }

        /*
         * Read gps messages from log files.
         */
        override protected void Listen()
        {
            Debug.WriteLine("GpsSymulator: Listen");
            DirectoryInfo gpsLogDirInfo = new DirectoryInfo(this.gpsLogDirPath);
            foreach (FileInfo gpslogFileInfo in gpsLogDirInfo.GetFiles("*.gps"))
            {
                Debug.WriteLine("GpsSymulator: Listen: reading gps log: " + gpslogFileInfo.ToString());
                string pathToGpsLog = this.gpsLogDirPath + "\\" + gpslogFileInfo.ToString();
                // create reader & open file
                TextReader gpsLogTr = new StreamReader(pathToGpsLog);
                string gpsMessage;
                while ((gpsMessage = gpsLogTr.ReadLine()) != null)
                {
                    //Debug.WriteLine("GpsSymulator: Listen: got gps message: " + gpsMessage);
                    parseGpsMessage(gpsMessage);
                    System.Threading.Thread.Sleep(200);
                }
                // close the stream
                gpsLogTr.Close();
            }
        }
    }
}
