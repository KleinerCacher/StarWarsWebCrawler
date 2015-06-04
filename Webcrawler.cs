using System;
using System.Collections.Generic;

namespace WebCrawler
{
    public class Webcrawler
    {
        private readonly Uri baseUrl;

        /// <summary>
        /// Initializes a new instance of the Webcrawler class.
        /// </summary>
        /// <param name="mainUrl"></param>
        public Webcrawler(Uri mainUrl)
        {
            this.baseUrl = mainUrl;
        }

        public void CrawlWeb()
        {
            AllIndexData data = new AllIndexData();
            data.SpaceShips = TransportationCrawler.CrawlTransportation(baseUrl, TransportationCrawler.Type.SpaceShip);
            ExcelGenerator.GenerateExcelFile("spaceships.xlsx", data.SpaceShips);

            data.Vehicles = TransportationCrawler.CrawlTransportation(baseUrl, TransportationCrawler.Type.Vehicle);
            ExcelGenerator.GenerateExcelFile("vehicles.xlsx", data.Vehicles);
            // XmlGenerator.GenerateXml("IndexData.xml", data);
        }
    }
}
