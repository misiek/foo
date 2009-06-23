using System;
using System.Collections.Generic;
using System.Text;
using Gps;
using System.Diagnostics;

namespace TouristGuide.map
{
    /**
     * This class contains logic for analyzing gps data.
     * It keep historical gps data;
     */
    public class GpsDataAnalyzer
    {
        // max number of gps location packages that can be stored
        private int capacity = 4;
        private List<GpsLocation> gpsLocationsCache = new List<GpsLocation>();


        public void addGpsLocation(GpsLocation gpsLoc)
        {
            gpsLocationsCache.Insert(0, gpsLoc);
            removeExcess();
        }

        private void removeExcess()
        {
            int excessCount = gpsLocationsCache.Count - capacity;
            if (excessCount > 0)
            {
                gpsLocationsCache.RemoveRange(capacity, excessCount);
            }
        }

        // estimates course base on cached locations
        public double estimateCourse()
        {
            if (gpsLocationsCache.Count < capacity)
            {
                // cache not ready
                return 0;
            }
            // secound point
            double lat2 = (gpsLocationsCache[0].getLatitude()+ gpsLocationsCache[1].getLatitude()) / 2;
            double lon2 = (gpsLocationsCache[0].getLongitude()+ gpsLocationsCache[1].getLongitude()) / 2;
            // first point
            double lat1 = (gpsLocationsCache[2].getLatitude() + gpsLocationsCache[3].getLatitude()) / 2;
            double lon1 = (gpsLocationsCache[2].getLongitude() + gpsLocationsCache[3].getLongitude()) / 2;

            // arcus tangens wspolczynnika kierunkowego prostej to kat miedzy prosta a osia ox
            double alfa = Math.Atan((lat2 - lat1) / (lon2 - lon1));

            double alfaDegree = alfa * 180 / Math.PI;

            double course = Math.Abs(alfaDegree);
            if (lon2 > lon1 && lat2 > lat1)
            {
                // first quarter
            }
            else if (lon2 > lon1 && lat2 < lat1)
            {
                // secound quarter
                course += 90;
            }
            else if (lon2 < lon1 && lat2 < lat1)
            {
                // third quarter
                course += 180;
            }
            else if (lon2 < lon1 && lat2 > lat1)
            {
                // fourth quarter
                course += 270;
            }

            return course;
        }
    }
}
