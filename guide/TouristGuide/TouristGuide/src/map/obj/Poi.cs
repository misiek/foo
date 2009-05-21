using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class Poi
    {
        private string lang;
        private double latitude;
        private double longitude;
        private string type;
        private string name;
        private string descr;
        private List<MediaFile> mediaFiles;
        private List<MainDetail> mainDetails;
        // to be able to update poi from portal
        private DateTime updated;

        public Poi(string name, double latitude, double longitude,
                   string lang, string type, string descr)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.lang = lang;
            this.type = type;
            this.descr = descr;
            this.mediaFiles = new List<MediaFile>();
            this.mainDetails = new List<MainDetail>();
        }

        public override string ToString()
        {
            return "<poi " + this.name + " [" + this.latitude + ", " + this.longitude + "]>";
        }

        public string getName()
        {
            return this.name;
        }

        public double getLatitude()
        {
            return this.latitude;
        }

        public double getLongitude()
        {
            return this.longitude;
        }

        public string getType()
        {
            return this.type;
        }

        public string getLang()
        {
            return this.lang;
        }

        public string getDescr()
        {
            return this.descr;
        }

        public List<MediaFile> getMediaFiles()
        {
            return this.mediaFiles;
        }

        public List<MainDetail> getMainDetails()
        {
            return this.mainDetails;
        }

        public DateTime getUpdated()
        {
            return this.updated;
        }

        public void setUpdated(DateTime updated)
        {
            this.updated = updated;
        }
    }
}
