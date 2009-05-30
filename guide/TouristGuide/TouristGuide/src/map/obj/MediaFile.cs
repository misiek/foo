using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TouristGuide.map.obj
{
    public class MediaFile
    {
        private string title;
        private string descr;
        private Uri url;
        private Object media;

        public MediaFile(string title, string descr, Uri url)
        {
            this.title = title;
            this.descr = descr;
            this.url = url;
        }

        public override string ToString()
        {
            return "<media_file " + this.title + ">";
        }

        public string getTitle()
        {
            return this.title;
        }

        public string getDescr()
        {
            return this.descr;
        }

        public Uri getUrl()
        {
            return this.url;
        }

        public void setMedia(Object media)
        {
            if (this.media != null)
            {
                throw new Exception("Media conflict, replacing media not allowed.");
            }
            this.media = media;
        }

        public Object getMedia()
        {
            return this.media;
        }
    }
}
