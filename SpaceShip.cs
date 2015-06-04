using HtmlAgilityPack;
using System;
using System.Linq;

namespace WebCrawler
{
    public class SpaceShip : Transportation
    {
        public string HyperDrive { get; set; }
        public string NaviComputer { get; set; }
        public string Consumables { get; set; }

        /// <summary>
        /// Initializes a new instance of the SpaceShip class.
        /// </summary>
        public SpaceShip()
            : base()
        {
            HyperDrive = String.Empty;
            NaviComputer = String.Empty;
            Consumables = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the SpaceShip class.
        /// </summary>
        public SpaceShip(TransportationMin transportMin, HtmlDocument htmlDocument)
            : base(transportMin, htmlDocument)
        {
            var itemDetailNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='item_detail']//span");
            HyperDrive = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Hyperdrive");
            NaviComputer = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Navicomputer");
            Consumables = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Consumables");
        }

        public static SpaceShip GetSpaceShip(TransportationMin transport)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(transport.Link);
            SpaceShip spaceShip = new SpaceShip(transport, htmlDocument);
            return spaceShip;
        }
    }
}
