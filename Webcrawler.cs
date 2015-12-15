using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace WebCrawler
{
    public class Webcrawler
    {
        const string filepathIndexedData = @"AllIndexedData.xml";

        private readonly Uri baseUrl;

        /// <summary>
        /// Initializes a new instance of the Webcrawler class.
        /// </summary>
        /// <param name="mainUrl"></param>
        public Webcrawler(Uri mainUrl)
        {
            this.baseUrl = mainUrl;
        }

        public void CrawlWeb(bool getNewData, bool generateExcel, bool generateTransportationList)
        {
            GetDataFromWeb();
            GenerateDataFromXml();       
            GenerateListOfTransportation();
        }

        private void GetDataFromWeb()
        {
            AllIndexData data = new AllIndexData();
            data.SpaceShips = TransportationCrawler.CrawlTransportation(baseUrl, TransportationCrawler.Type.SpaceShip);
            data.Vehicles = TransportationCrawler.CrawlTransportation(baseUrl, TransportationCrawler.Type.Vehicle);

            Serialize(data);
        }

        private void Serialize(AllIndexData data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AllIndexData));

            using (TextWriter WriteFileStream = new StreamWriter(filepathIndexedData))
            {
                serializer.Serialize(WriteFileStream, data);
            }
        }

        private void GenerateDataFromXml()
        {
            AllIndexData data = Deserialize();

            ExcelGenerator.GenerateExcelFile("spaceships.xlsx", data.SpaceShips);
            ExcelGenerator.GenerateExcelFile("vehicles.xlsx", data.Vehicles);
        }

        private AllIndexData Deserialize()
        {
            using (FileStream readFileStream = new FileStream(filepathIndexedData, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AllIndexData));
                return (AllIndexData)serializer.Deserialize(readFileStream);
            }
        }

        private void GenerateListOfTransportation()
        {
            AllIndexData data = Deserialize();
            List<string> namesOfShips = new List<string>();
            namesOfShips.AddRange(data.SpaceShips.Select(s => s.Name));
            namesOfShips.AddRange(data.Vehicles.Select(v => v.Name));

            File.WriteAllLines("Transportation.txt", namesOfShips);
        }
    }
}
