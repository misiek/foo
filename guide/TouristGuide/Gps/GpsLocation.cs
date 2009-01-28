using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Gps
{
    public class GpsLocation
    {
        // time recived from gps
        private DateTime satelliteTime;
        // latitude string eg. '50.05994539169559 N'
        private string latitudeString;
        // latitude converted to double positive for N negative for S
        private double latitude;
        // longitude string eg. 19.941730499267578 E
        private string longitudeString;
        // longitude converted to double positive for E negative for W
        private double longitude;
        
        private double speed;
        private double course;
        //private string status;

        public GpsLocation(DateTime satelliteTime,
                            string latitudeString,
                            string longitudeString,
                            double speed,
                            double course) 
        {
            this.satelliteTime = satelliteTime;
            // latitude
            this.latitudeString = parseCoordinateStr(latitudeString, false);
            this.latitude = parseCoordinate(latitudeString);
            // longitude
            this.longitudeString = parseCoordinateStr(longitudeString, true);
            this.longitude = parseCoordinate(longitudeString);
            // speed
            this.speed = speed;
            // course
            this.course = course;
        }

        private double parseCoordinate(string coordinateStr)
        {
            if (coordinateStr == null)
                return 1000;
            string[] coordinateArr = coordinateStr.Split(' ');
            double angle = Convert.ToDouble(coordinateArr[0]);
            // move dot two positions left
            angle /= 100;
            char indicator = coordinateArr[1].ToCharArray()[0];
            if ('W' == indicator || 'S' == indicator)
                angle *= -1;
            return angle;
        }

        private string parseCoordinateStr(string coordinateStr, bool wantLongitute)
        {
            if (coordinateStr == null)
                return "unknown";
            int degreeLength = 2;
            if (wantLongitute)
                degreeLength++;
            // Append hours
            string parsed = coordinateStr.Substring(0, degreeLength) + "°";
            // Append minutes
            int spaceIndex = coordinateStr.IndexOf(" ");
            parsed += coordinateStr.Substring(degreeLength, spaceIndex - degreeLength) + "\"";
            parsed += coordinateStr.Substring(spaceIndex);
            return parsed;
        }

        public string getLatitudeString()
        {
            //Debug.WriteLine("getLatitudeString: " + this.latitudeString + " double: " + this.latitude, this.ToString());
            return this.latitudeString;
        }

        public string getLongitudeString()
        {
            //Debug.WriteLine("getLongitudeString: " + this.longitudeString + " double: " + this.longitude, this.ToString());
            return this.longitudeString;
        }

        public double getSpeed()
        {
            return this.speed;
        }
    }
}
