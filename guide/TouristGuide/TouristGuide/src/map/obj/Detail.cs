using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public abstract class Detail
    {
        protected string title;
        protected string descr;
        protected List<MediaFile> mediaFiles;

        protected Detail(string title, string descr)
        {
            this.title = title;
            this.descr = descr;
            this.mediaFiles = new List<MediaFile>();
        }

        public string getTitle()
        {
            return this.title;
        }

        public string getDescr()
        {
            return this.descr;
        }

        public List<MediaFile> getMediaFiles()
        {
            return this.mediaFiles;
        }

    }
}
