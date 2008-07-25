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
        public double SpeedMPH;
        public double Course;
        public string Status;

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

        private void RMC(string[] words)
        {
            sateliteTime(words[1]);

            RecivedData.Status = status(words[2]);

            // Extract latitude and longitude
            RecivedData.LatitudeString = coordinateString(words[3], words[4]);
            RecivedData.LongitudeString = coordinateString(words[5], words[6]);

            RecivedData.SpeedMPH = speed(words[7], MPHPerKnot);

            // extract bearing
            RecivedData.Course = course(words[8]);

            DumpGpsData();
        }

        private void DumpGpsData()
        {
            Console.WriteLine("gpsMessage: " + GpsSentence);
            Console.WriteLine("RecivedData.SateliteTime: " + RecivedData.SateliteTime);
            Console.WriteLine("RecivedData.Status: " + RecivedData.Status);
            Console.WriteLine("RecivedData.SpeedMPH: " + RecivedData.SpeedMPH);
            Console.WriteLine("RecivedData.LatitudeString: " + RecivedData.LatitudeString);
            Console.WriteLine("RecivedData.LongitudeString: " + RecivedData.LongitudeString);
        }

        private string status(string wordStatus)
        {
            string status = "";
            if (wordStatus != "")
            {
                switch (wordStatus)
                {
                    case "A":
                        //  FixObtained();
                        status = "A";
                        break;
                    case "V":
                        //  FixLost();
                        status = "V";
                        break;
                }
            }
            return status;
        }

        private double course(string wordCourse)
        {
            double course = 0;
            if (wordCourse != "")
            {
                course = double.Parse(wordCourse, NmeaCultureInfo);
            }
            return course;
        }

        private double speed(string wordSpeed, double scaler)
        {
            double speed = 0;
            if (wordSpeed != "")
            {
                // Yes.  Parse the speed and convert it to MPH
                speed = double.Parse(wordSpeed, NmeaCultureInfo) * scaler;
            }
            return speed;
        }

        private string coordinateString(string wordCoordinate, string wordIndicator) 
        {
            string coordinate = "";
            if (wordCoordinate != "" && wordIndicator != "")
            {
                // Append hours
                coordinate = wordCoordinate.Substring(0, 2) + "°";
                // Append minutes
                coordinate = coordinate + wordCoordinate.Substring(2) + "\"";
                // Append the hemisphere
                coordinate = coordinate + wordIndicator; 
            }
            return coordinate;
        }

        private void sateliteTime(string timeWord)
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
