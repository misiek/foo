using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GpsConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            MyGPS gps = new MyGPS();
            gps.Open();
            gps.read();

            //while(true)
            //{

                
                
            //}


        }
    }
}
