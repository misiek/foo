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
        private byte[] mediaBytes;

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

        public void setMedia(byte[] mediaBytes)
        {
            if (this.mediaBytes != null)
            {
                throw new Exception("Media conflict, replacing media not allowed.");
            }
            this.mediaBytes = mediaBytes;
        }

        public byte[] getMedia()
        {
            return this.mediaBytes;
        }

        public string getFileName()
        {
            return this.url.Segments[this.url.Segments.Length - 1];
        }
    }
}
