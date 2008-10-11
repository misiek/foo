using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GpsGuide.Gps
{
    public class LocationData
    {
        private DateTime SatelliteTime;
        private string LatitudeString;
        private string LongitudeString;
        private double Speed;
        private double Course;
        private string Status;

        public LocationData(
            DateTime SatelliteTime,
            string LatitudeString,
            string LongitudeString,
            double Speed,
            double Course
        ) {
            this.SatelliteTime      = SatelliteTime;
            this.LatitudeString     = LatitudeString;
            this.LongitudeString    = LongitudeString;
            this.Speed              = Speed;
            this.Course             = Course;
        }

        public string getLatitudeString()
        {
            return LatitudeString;
        }

        public string getLongitudeString()
        {
            return LongitudeString;
        }

        public string getSpeed()
        {
            return Speed.ToString();
        }
    }
}
