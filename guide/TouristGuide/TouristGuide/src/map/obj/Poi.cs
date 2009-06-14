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

        public Poi(string name, double latitude, double longitude)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.lang = null;
            this.type = null;
            this.descr = null;
            this.mediaFiles = new List<MediaFile>();
            this.mainDetails = new List<MainDetail>();
        }

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

        public bool isDataFree()
        {
            return this.descr == null;
        }

        public void freeData()
        {
            this.descr = null;
            this.lang = null;
            this.type = null;
        }

        public void insertData(string descr, string lang, string type)
        {
            if (this.lang == null && this.type == null && this.descr == null)
            {
                this.lang = lang;
                this.type = type;
                this.descr = descr;
            }
        }

        public bool isMediaFree()
        {
            return this.mediaFiles.Count == 0 && this.mainDetails.Count == 0;
        }

        public void freeMedia()
        {
            //this.mediaFiles.Clear();
            //this.mainDetails.Clear();
            this.mediaFiles = new List<MediaFile>();
            this.mainDetails = new List<MainDetail>();
        }

        public void free()
        {
            if (!isDataFree())
                freeData();
            if (!isMediaFree())
                freeMedia();
        }

        public override string ToString()
        {
            return this.name;
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

        /**
         * Returns all media files, also from main details.
         */
        public List<MediaFile> getAllMediaFiles()
        {
            List<MediaFile> allMediaFiles = new List<MediaFile>();
            // poi's media files
            allMediaFiles.AddRange(getMediaFiles());
            // media files form details
            foreach (MainDetail md in getMainDetails())
            {
                allMediaFiles.AddRange(md.getMediaFiles());
            }
            return allMediaFiles;
        }
    }
}
