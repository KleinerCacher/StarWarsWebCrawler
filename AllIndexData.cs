using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebCrawler
{
    [Serializable, XmlInclude(typeof(Vehicle)), XmlInclude(typeof(SpaceShip))]
    public class AllIndexData
    {
        public List<IndexData> SpaceShips { get; set; }
        public List<IndexData> Vehicles { get; set; }
    }
}
