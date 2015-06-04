using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebCrawler
{
    public class TransportationCrawler
    {
        public enum Type
        {
            SpaceShip,
            Vehicle
        }

        private const string vehicleUrl = "transportation/vehicles/?order_by=name&flattened=true";
        private const string spaceShipUrl = "transportation/starships/?order_by=name&flattened=true";

        public static List<IndexData> CrawlTransportation(Uri baseUrl, Type type)
        {
            string typeUrl = string.Empty;
            switch (type)
            {
                case Type.SpaceShip:
                    typeUrl = spaceShipUrl;
                    break;
                case Type.Vehicle:
                    typeUrl = vehicleUrl;
                    break;
            }

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(new Uri(baseUrl, typeUrl).ToString());

            int nameIndex = GetIndexOfColumn(htmlDocument, "Item");
            int costIndex = GetIndexOfColumn(htmlDocument, "Cost");
            int rarityIndex = GetIndexOfColumn(htmlDocument, "Rarity");

            var transMins = from table in htmlDocument.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                            from row in table.SelectNodes("tr").Where(x => !x.HasAttributes).Cast<HtmlNode>()
                            select new TransportationMin()
                           {
                               Name = row.SelectNodes("td")[nameIndex].InnerText,
                               Cost = row.SelectNodes("td")[costIndex].InnerText,
                               Rarity = row.SelectNodes("td")[rarityIndex].InnerText,
                               Link = new Uri(baseUrl, row.SelectNodes("td")[nameIndex].SelectSingleNode("a").Attributes["href"].Value).ToString()
                           };


            List<IndexData> transports = new List<IndexData>();
            int i = 0;
            foreach (TransportationMin trans in transMins)
            {
                switch (type)
                {
                    case Type.SpaceShip:
                        transports.Add(SpaceShip.GetSpaceShip(trans));
                        break;
                    case Type.Vehicle:
                        transports.Add(Vehicle.GetVehicle(trans));
                        break;
                }

                if (i == 2)
                {
                    break;
                }
                i++;
            }

            return transports;
        }

        private static int GetIndexOfColumn(HtmlDocument htmlDocument, string columnName)
        {
            HtmlNodeCollection headerNodes = htmlDocument.DocumentNode.SelectNodes("//tr[@class='sortable_header']//td");
            int index = 0;
            foreach (HtmlNode node in headerNodes)
            {
                if (node.InnerText.Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return index;
                }

                index++;
            }

            throw new ArgumentOutOfRangeException("HeaderNode not available");
        }
    }
}
