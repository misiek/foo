using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.Globalization;
namespace GpsConsole
{
    struct RecivedGpsData
    {
        public DateTime SateliteTime;
        public string LatitudeString;
        public string LongitudeString;
        public double Speed;
        public double Course;
        public string Status;
        public int SatellitesUsed;

    }

    class NmeaParser
    {
        public const short UNRECOGNIZED = 0;
        public const short LOCATION = 1;
        public const short SATELITE = 2;


        // Represents the EN-US culture, used for numers in NMEA sentences
        public static CultureInfo NmeaCultureInfo = new CultureInfo("en-US");
        // Used to convert knots into miles per hour
        private static double MPHPerKnot = double.Parse("1.150779", NmeaCultureInfo);
      

        private RecivedGpsData RecivedData = new RecivedGpsData();
        private string GpsSentence = "";

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
        public short Parse(string gpsMessage)
        {
            short status;
            Regex rxChecksum = new Regex("[*].+?$");
            Regex rxGps = new Regex("^[$]GP");

            GpsSentence = gpsMessage;
            gpsMessage = gpsMessage.Trim();
            //gpsMessage = rxChecksum.Replace(gpsMessage, "");
            string[] msg = gpsMessage.Split('*');
            //Console.WriteLine("gpsMessage checksum: " + msg[1]);
            //Console.WriteLine("gpsMessage: " + msg[0]);

            string[] words = msg[0].Split(',');
            if (rxGps.IsMatch(words[0]))
            {
                string msg_type = rxGps.Replace(words[0], "");
                switch (msg_type)
                {
                    case "GGA":
                        GGA(words);
                        status = LOCATION;
                        break;
                    case "RMC":
                        // RMC – Recommended minimum data for gps
                        RMC(words);
                        status = LOCATION;
                        break;
                    case "GSV":
                        // GSV – Detailed Satellite data
                        GSV(words);
                        status = SATELITE;
                        break;
                    case "GSA":
                        // GSA – Overall Satellite data
                        GSA(words);
                        status = SATELITE;
                        break;
                    default:
                        Debug.WriteLine("unrecognized message type: " + words[0], this.ToString());
                        status = UNRECOGNIZED;
                        break;
                }
            }
            else
            {
                Debug.WriteLine("not gps sentence: " + words[0], this.ToString());
                status = UNRECOGNIZED;
            }
            return status;
        }

        private void GGA(string[] words)
        {
            updateSatelliteTime(words[1]);

            // Extract latitude and longitude
            updateLatitude(words[2], words[3]);
            updateLongitude(words[4], words[5]);

            updateSatellitesUsed(words[7]);

            DumpGpsData();
        }

        private void updateSatellitesUsed(string wordSatelites)
        {
            if (wordSatelites != "")
            {
                int satellitesCount = int.Parse(wordSatelites, NmeaCultureInfo);
                RecivedData.SatellitesUsed = satellitesCount;
            }
        }

        private void RMC(string[] words)
        {
            updateSatelliteTime(words[1]);

            updateStatus(words[2]);

            // Extract latitude and longitude
            updateLatitude(words[3], words[4]);
            updateLongitude(words[5], words[6]);

            updateSpeed(words[7], MPHPerKnot);

            // extract bearing
            updateCourse(words[8]);

            DumpGpsData();
        }

        private void DumpGpsData()
        {
            Console.WriteLine("gpsMessage: " + GpsSentence);
            Console.WriteLine("RecivedData.SateliteTime: " + RecivedData.SateliteTime);
            Console.WriteLine("RecivedData.Status: " + RecivedData.Status);
            Console.WriteLine("RecivedData.Speed: " + RecivedData.Speed);
            Console.WriteLine("RecivedData.LatitudeString: " + RecivedData.LatitudeString);
            Console.WriteLine("RecivedData.LongitudeString: " + RecivedData.LongitudeString);
            Console.WriteLine("RecivedData.SatellitesUsed: " + RecivedData.SatellitesUsed);
        }

        private void updateStatus(string wordStatus)
        {
            if (wordStatus != "")
            {
                switch (wordStatus)
                {
                    case "A":
                        //  FixObtained();
                        RecivedData.Status = "A";
                        break;
                    case "V":
                        //  FixLost();
                        RecivedData.Status = "V";
                        break;
                }
            }
        }

        private void updateCourse(string wordCourse)
        {
            if (wordCourse != "")
            {
                RecivedData.Course = double.Parse(wordCourse, NmeaCultureInfo);
            }
        }

        private void updateSpeed(string wordSpeed, double scaler)
        {
            if (wordSpeed != "")
            {
                // Parse the speed and convert it
                RecivedData.Speed = double.Parse(wordSpeed, NmeaCultureInfo) * scaler;
            }
        }

        private void updateLatitude(string wordLatitude, string wordIndicator)
        {
            if (wordLatitude != "" && wordIndicator != "")
            {
                // Append hours
                string latitude = wordLatitude.Substring(0, 2) + "°";
                // Append minutes
                latitude = latitude + wordLatitude.Substring(2) + "\"";
                // Append the hemisphere
                latitude = latitude + wordIndicator;
                RecivedData.LatitudeString = latitude;
            }
        }

        private void updateLongitude(string wordLongitude, string wordIndicator)
        {
            if (wordLongitude != "" && wordIndicator != "")
            {
                // Append hours
                string longitude = wordLongitude.Substring(0, 2) + "°";
                // Append minutes
                longitude = longitude + wordLongitude.Substring(2) + "\"";
                // Append the hemisphere
                longitude = longitude + wordIndicator;
                RecivedData.LongitudeString = longitude;
            }
        }

        //private string coordinateString(string wordCoordinate, string wordIndicator) 
        //{
        //    string coordinate = "";
        //    if (wordCoordinate != "" && wordIndicator != "")
        //    {
        //        // Append hours
        //        coordinate = wordCoordinate.Substring(0, 2) + "°";
        //        // Append minutes
        //        coordinate = coordinate + wordCoordinate.Substring(2) + "\"";
        //        // Append the hemisphere
        //        coordinate = coordinate + wordIndicator; 
        //    }
        //    return coordinate;
        //}

        private void updateSatelliteTime(string timeWord)
        {
            if (timeWord != "")
            {
                // Extract hours, minutes, seconds and milliseconds
                int UtcHours = Convert.ToInt32(timeWord.Substring(0, 2));
                int UtcMinutes = Convert.ToInt32(timeWord.Substring(2, 2));
                int UtcSeconds = Convert.ToInt32(timeWord.Substring(4, 2));
                int UtcMilliseconds = 0;
                // Extract milliseconds if it is available
                if (timeWord.Length > 7)
                {
                    UtcMilliseconds = Convert.ToInt32(timeWord.Substring(7));
                }
                // Now build a DateTime object with all values
                System.DateTime Today = System.DateTime.Now.ToUniversalTime();
                System.DateTime SatelliteTime = new System.DateTime(Today.Year,
                  Today.Month, Today.Day, UtcHours, UtcMinutes, UtcSeconds,
                  UtcMilliseconds);
                RecivedData.SateliteTime = SatelliteTime.ToLocalTime();
            }
        }

        private void GSV(string[] words)
        {

        }

        private void GSA(string[] words)
        {

        }

    }
}
