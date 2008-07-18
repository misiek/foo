using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;

namespace GpsConsole
{
    class NmeaParser
    {
        private Regex rxChecksum = new Regex("[*].+?$");

        /*
         * Message types:
            · AAM – Waypoint Arrival Alarm,
            · ALM – Almanac data,
            · APA – Auto Pilot A sentence,
            · APB – Auto Pilot B sentence,
            · BOD – Bearing Origin to Destination,
            · BWC – Bearing using Great Circle route,
            · DTM – Datum being used,
            · GGA – Fix information,
            · GLL – Lat/Lon data,
            · GSA – Overall Satellite data,
            · GSV – Detailed Satellite data,
            · MSK – Send control for a beacon receiver,
            · MSS – Beacon receiver status information,
            · RMA – Recommended Loran data,
            · RMB – Recommended navigation data for gps,
            · RMC – Recommended minimum data for gps,
            · RTE – Route message,
            · VTG – Vector track an Speed over the Ground,
            · WCV – Waypoint closure velocity (Velocity Made Good),
            · WPL – Waypoint information,
            · XTC – Cross track error,
            · XTE – Measured cross track error,
            · ZTG – Zulu (UTC) time and time to go (to destination),
            · ZDA – Date and Time.
         * */
        public void Parse(string gpsMessage)
        {
            gpsMessage = gpsMessage.Trim();
            //gpsMessage = rxChecksum.Replace(gpsMessage, "");
            string[] msg = gpsMessage.Split('*');
            Console.WriteLine("gpsMessage checksum: " + msg[1]);
            Console.WriteLine("gpsMessage: " + msg[0]);

            string[] words = msg[0].Split(',');
            switch (words[0])
            {
                case "$GPRMC":
                    // RMC – Recommended minimum data for gps
                    ParseGPRMC(words);
                    break;
                case "$GPGSV":
                    // GSV – Detailed Satellite data
                    ParseGPGSV(words);
                    break;
                case "$GPGSA":
                    // GSA – Overall Satellite data
                    ParseGPGSA(words);
                    break;
                default:
                    Debug.WriteLine("unrecognized message type: " + words[0], this.ToString());
            }

        }

        private void ParseGPRMC(string[] words)
        {

        }

        private void ParseGPGSV(string[] words)
        {

        }

        private void ParseGPGSA(string[] words)
        {

        }

    }
}
