using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;

using TouristGuide.map.obj;
using System.IO;
using TouristGuide.map.exception;

namespace TouristGuide.map
{
    public class Portal
    {
        private Config config;
        public Config Config
        {
            set
            {
                this.config = value;
            }
        }

        public void getAreas()
        {
            throw new System.NotImplementedException();
        }

        public String getPois(Area area)
        {
            string portalUrl = this.config.get("portal_url");
            string mobappid = this.config.get("mobappid");
            string areaStr = area.getTopLeftLatitude() + "," + area.getTopLeftLongitude() +
                "," + area.getBottomRightLatitude() + "," + area.getBottomRightLongitude();
            string queryString = portalUrl + "/pois/area/" + areaStr + 
                "/lang/pl/?mobappid=" + mobappid;

            Debug.WriteLine("getPois: queryString: " + queryString, ToString());
            return GET(new Uri(queryString));
        }

        private string GET(Uri u)
        {
            string data = null;

            WebRequest request = WebRequest.Create(u);
            request.Timeout = 15000; // 15 seconds in milliseconds
            //request.Proxy = null;
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                data = reader.ReadToEnd();
            }
            catch (WebException e)
            {
                Debug.WriteLine("GET: connection error: " + e.Message, ToString());
                throw new PortalException("Connection error", e);
            }
            return data;
        }

        public byte[] download(Uri u)
        {
            byte[] downloadedData = new byte[0];
            try
            {

                //Get a data stream from the url
                WebRequest req = WebRequest.Create(u);
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();

                //Download in chuncks
                byte[] buffer = new byte[1024];

                //Get Total Size
                int dataLength = (int)response.ContentLength;

                //Download to memory
                //Note: adjust the streams here to download directly to the hard drive
                MemoryStream memStream = new MemoryStream();
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //Write the downloaded data
                    memStream.Write(buffer, 0, bytesRead);
                }

                //Convert the downloaded stream to a byte array
                downloadedData = memStream.ToArray();

                //Clean up
                stream.Close();
                memStream.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("download: error: " + e.Message, ToString());
                throw new PortalException("Donwload error", e);
            }
            return downloadedData;
        }

    }
}
