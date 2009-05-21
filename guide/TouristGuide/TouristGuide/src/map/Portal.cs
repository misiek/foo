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
            return GET(queryString);
        }

        private String GET(string url)
        {
            String responseStr = null;

            WebRequest request = WebRequest.Create(url);
            //request.Proxy = null;
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseStr = reader.ReadToEnd();
            }
            catch (WebException e)
            {
                Debug.WriteLine("GET: connection error: " + e.Message, ToString());
                throw new PortalException("Connection error", e);
            }

            return responseStr;
        }
    }
}
