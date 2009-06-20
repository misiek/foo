using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Collections;
using System.Diagnostics;

namespace TouristGuide.map
{
    public class Targets
    {
        private List<Poi> targets = new List<Poi>();
        private Hashtable targetsAreas = new Hashtable();

        private double maxLatitudeToTarget = 0.000138;
        private double maxLongitudeToTarget = 0.000215;

        public delegate void TargetDone(Poi target);
        public event TargetDone targetDone;

        public void add(Poi target)
        {
            Debug.WriteLine("add: " + target, this.ToString());
            lock (targets)
            {
                targets.Add(target);
                Area tArea = createTargetArea(target);
                targetsAreas[target] = tArea;
            }
        }

        private Area createTargetArea(Poi target)
        {
            double topLeftLat = target.getLatitude() + maxLatitudeToTarget;
            double topLeftLon = target.getLongitude() - maxLongitudeToTarget;
            double bottomRightLat = target.getLatitude() - maxLatitudeToTarget;
            double bottomRightLon = target.getLongitude() + maxLongitudeToTarget;
            return new Area(topLeftLat, topLeftLon, bottomRightLat, bottomRightLon);
        }

        public void remove(Poi target)
        {
            Debug.WriteLine("remove: " + target, this.ToString());
            lock (targets)
            {
                targets.Remove(target);
                targetsAreas.Remove(target);
            }
        }

        public void move(Poi target, int move)
        {
            Debug.WriteLine("move: " + target + ", move: " + move, this.ToString());
            lock (targets)
            {
                int index = targets.IndexOf(target);
                int moveIndex = index + move;
                Poi tmp = targets[moveIndex];
                targets[moveIndex] = target;
                targets[index] = tmp;
            }
        }

        public Poi getCurrent(double latitude, double longitude)
        {
            Debug.WriteLine("getCurrent: latitude: " + latitude
                + ", longitude: " + longitude, this.ToString());
            if (targets.Count == 0)
                return null;
            lock (targets)
            {
                Poi currentTarget = targets[0];
                Area currentArrea = (Area)targetsAreas[currentTarget];
                if (currentArrea.contains(latitude, longitude)) {
                    targets.Remove(currentTarget);
                    targetsAreas.Remove(currentTarget);
                    if (targetDone != null)
                        targetDone(currentTarget);
                }
                return currentTarget;
            }
        }

    }
}
