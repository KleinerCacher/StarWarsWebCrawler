using HtmlAgilityPack;
using System;

namespace WebCrawler
{
    public class Vehicle : Transportation
    {
        public Vehicle()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Vehicle class.
        /// </summary>
        public Vehicle(TransportationMin transportMin, HtmlDocument htmlDocument)
            : base(transportMin, htmlDocument)
        {
        }

        public static Vehicle GetVehicle(TransportationMin transportMin)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(transportMin.Link);
            Vehicle vehicle = new Vehicle(transportMin, htmlDocument);


            return vehicle;
        }
    }
}
