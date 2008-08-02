using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.Globalization;
using System.Collections;

namespace GpsConsole
{
    struct RecivedGpsData
    {
        public DateTime SatelliteTime;
        public string LatitudeString;
        public string LongitudeString;
        public double Speed;
        public double Course;
        public string Status;
        public int SatellitesUsed;

        // Satellites
        public double PDOP;
        public double HDOP;
        public double VDOP;
        public int SatellitesInView;
        public Hashtable SatellitesDetails;
        public int[] SatellitesChannels;
    }

    struct Satellite
    {
        public int SatelliteID;
        public int Elevation;
        public int Azimuth;
        public int SignalToNoiseRatio;
        public int Channel;

        public Satellite(int SatelliteID, int Elevation, int Azimuth, int SignalToNoiseRatio)
        {
            this.SatelliteID = SatelliteID;
            this.Elevation = Elevation;
            this.Azimuth = Azimuth;
            this.SignalToNoiseRatio = SignalToNoiseRatio;
            this.Channel = 0;
        }
    }

    class NmeaParser
    {
        public const short UNRECOGNIZED = 0;
        public const short LOCATION = 1;
        public const short SATELLITE = 2;
        public const short CHECKSUM_INVALID = 3;


        // Represents the EN-US culture, used for numers in NMEA sentences
        public static CultureInfo NmeaCultureInfo = new CultureInfo("en-US");
        // Used to convert knots into miles per hour
        private static double MPHPerKnot = double.Parse("1.150779", NmeaCultureInfo);
      

        private RecivedGpsData RecivedData;
        

        private string GpsSentence = "";


        public NmeaParser()
        {

            RecivedData = new RecivedGpsData();
            RecivedData.SatellitesDetails = new Hashtable();
            RecivedData.SatellitesChannels = new int[11];

        }


        public short Parse(string gpsMessage)
        {
            short status;
            Regex rxChecksum = new Regex("[*].+?$");
            Regex rxGps = new Regex("^[$]GP");

            gpsMessage = gpsMessage.Trim();

            if (isChecksumValid(gpsMessage))
            {
                GpsSentence = gpsMessage;
                Debug.WriteLine("");
                Debug.WriteLine("gpsMessage: " + GpsSentence, this.ToString());

                string[] msg = gpsMessage.Split('*');
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
                        case "GLL":
                            GLL(words);
                            status = LOCATION;
                            break;
                        case "GSV":
                            // GSV – Detailed Satellite data
                            GSV(words);
                            status = SATELLITE;
                            break;
                        case "GSA":
                            // GSA – Overall Satellite data
                            GSA(words);
                            status = SATELLITE;
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
            }
            else // checksum invalid
            {
                Debug.WriteLine("checksum error for: " + gpsMessage, this.ToString());
                status = CHECKSUM_INVALID;
            }
            return status;
        }

        // Returns True if a sentence's checksum matches the
        // calculated checksum
        public bool isChecksumValid(string gpsMessage)
        {
            string checksum_counted = getChecksum(gpsMessage);
            string checksum_got = gpsMessage.Substring(gpsMessage.IndexOf("*") + 1);
            return checksum_counted == checksum_got;
        }

        public string getChecksum(string gpsMessage)
        {
            // Loop through all chars to get a checksum
            int checksum = 0;
            foreach (char c in gpsMessage)
            {
                if (c == '$')
                {
                    // Ignore the dollar sign
                    continue;
                }
                else if (c == '*')
                {
                    // Stop processing before the asterisk
                    break;
                }
                // Is this the first value for the checksum?
                if (checksum == 0)
                {
                    // Set the checksum to the value
                    checksum = Convert.ToByte(c);
                }
                else
                {
                    // XOR the checksum with this character's value
                    checksum = checksum ^ Convert.ToByte(c);
                }
            }
            // return two character hex
            return checksum.ToString("X2");
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

        private void GLL(string[] words)
        {
            updateLatitude(words[1], words[2]);
            updateLongitude(words[3], words[4]);
            updateSatelliteTime(words[5]);
            updateStatus(words[6]);

            DumpGpsData();
        }

        private void GSV(string[] words)
        {
            updateSatellitesInView(words[3]);

            // update satelites details
            int NumberOfMessages = Convert.ToInt32(words[1]);
            //int MessageNumber = Convert.ToInt32(words[2]);
            for (int i = 1; i <= NumberOfMessages; i++)
            {
                int index = i * 4;
                if (words.Length < index+3)
                    break;
                updateSatellitesDetails(words[index], words[index + 1], words[index + 2], words[index + 3]);
            }

            DumpGpsData();
        }

        private void GSA(string[] words)
        {
            RecivedData.SatellitesChannels = new int[11];
            for (int i = 0; i < 11; i++)
            {
                if (words[i + 3] != "")
                {
                    int satelliteID = System.Convert.ToInt32(words[i + 3]);
                    if (RecivedData.SatellitesDetails.ContainsKey(satelliteID))
                    {
                        Satellite s = (Satellite) RecivedData.SatellitesDetails[satelliteID];
                        s.Channel = i + 1;
                    }
                    RecivedData.SatellitesChannels[i] = satelliteID;
                }
            }
            if (words[15] != "")
                RecivedData.PDOP = double.Parse(words[15], NmeaCultureInfo);
            if (words[16] != "")
                RecivedData.HDOP = double.Parse(words[16], NmeaCultureInfo);
            if (words[17] != "")
                RecivedData.VDOP = double.Parse(words[17], NmeaCultureInfo);
            
            DumpGpsData();
        }

        private void updateSatellitesDetails(string wordSatelliteID, string wordElevation, 
            string wordAzimuth, string wordSignalToNoiseRatio)
        {
            if (wordSatelliteID != "")
            {
                int SatelliteID = System.Convert.ToInt32(wordSatelliteID);
                int Elevation = -1;
                int Azimuth = -1;
                int SignalToNoiseRatio = -1;

                if (wordElevation != "")
                    Elevation = Convert.ToInt32(wordElevation);
                if (wordAzimuth != "")
                    Azimuth = Convert.ToInt32(wordAzimuth);
                if (wordSignalToNoiseRatio != "")
                    SignalToNoiseRatio = Convert.ToInt32(wordSignalToNoiseRatio);

                if (RecivedData.SatellitesDetails.ContainsKey(SatelliteID) ) {
                    Satellite s = (Satellite) RecivedData.SatellitesDetails[SatelliteID];
                    if (Elevation != -1)
                        s.Elevation = Elevation;
                    if (Azimuth != -1)
                        s.Azimuth = Azimuth;
                    if (SignalToNoiseRatio != -1)
                        s.SignalToNoiseRatio = SignalToNoiseRatio;
                } else {
                    RecivedData.SatellitesDetails[SatelliteID] = new Satellite(SatelliteID, Elevation, Azimuth, SignalToNoiseRatio);
                }
            }
        }

        private void updateSatellitesInView(string wordSatelitesInView) 
        {
            if (wordSatelitesInView != "")
            {
                RecivedData.SatellitesInView = int.Parse(wordSatelitesInView, NmeaCultureInfo);
            }
        }

        private void DumpGpsData()
        {
            Debug.WriteLine("Position:", this.ToString());
            Debug.WriteLine("RecivedData.SateliteTime: " + RecivedData.SatelliteTime, this.ToString());
            Debug.WriteLine("RecivedData.Status: " + RecivedData.Status, this.ToString());
            Debug.WriteLine("RecivedData.Speed: " + RecivedData.Speed, this.ToString());
            Debug.WriteLine("RecivedData.Course: " + RecivedData.Course, this.ToString());
            Debug.WriteLine("RecivedData.LatitudeString: " + RecivedData.LatitudeString, this.ToString());
            Debug.WriteLine("RecivedData.LongitudeString: " + RecivedData.LongitudeString, this.ToString());
            Debug.WriteLine("RecivedData.SatellitesUsed: " + RecivedData.SatellitesUsed, this.ToString());
            Debug.WriteLine("Satellites:", this.ToString());
            Debug.WriteLine("RecivedData.SatellitesInView: " + RecivedData.SatellitesInView, this.ToString());
            Debug.WriteLine("RecivedData.PDOP: " + RecivedData.PDOP);
            Debug.WriteLine("RecivedData.HDOP: " + RecivedData.HDOP);
            Debug.WriteLine("RecivedData.VDOP: " + RecivedData.VDOP);
            string keys = "";
            foreach (int sid in RecivedData.SatellitesDetails.Keys)
            {
                keys += sid + " ";
                //Satellite s = (Satellite) RecivedData.SatellitesDetails[sid];
                //Debug.WriteLine("s.SatelliteID: " + s.SatelliteID);
                //Debug.WriteLine("s.Elevation: " + s.Elevation);
                //Debug.WriteLine("s.Azimuth: " + s.Azimuth);
                //Debug.WriteLine("s.SignalToNoiseRatio: " + s.SignalToNoiseRatio);
                //Debug.WriteLine("s.Channel: " + s.Channel);
            }
            Debug.WriteLine("RecivedData.SatellitesDetails.Keys: " + keys, this.ToString());
            string channels = "";
            foreach (int sid in RecivedData.SatellitesChannels)
            {
                channels += sid + " ";
            }
            Debug.WriteLine("RecivedData.SatellitesChannels: " + channels, this.ToString());
        }

        private void updateSatellitesUsed(string wordSatelites)
        {
            if (wordSatelites != "")
            {
                int satellitesCount = int.Parse(wordSatelites, NmeaCultureInfo);
                RecivedData.SatellitesUsed = satellitesCount;
            }
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
                RecivedData.SatelliteTime = SatelliteTime.ToLocalTime();
            }
        }

    }
}
