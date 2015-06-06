using HtmlAgilityPack;
using System.Linq;
using System.Web;
using System;
using System.Globalization;
using System.Collections.Generic;

namespace WebCrawler
{
    public class Transportation : IndexData
    {
        public string Silhouette { get; set; }
        public string Speed { get; set; }
        public string Handling { get; set; }
        public string DefenseFore { get; set; }
        public string DefensePort { get; set; }
        public string DefenseStarboard { get; set; }
        public string DefenseAft { get; set; }
        public int DefenseMaximum { get; set; }
        public string Armor { get; set; }
        public string HullTrauma { get; set; }
        public string SystemStrain { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string MaximumAltitude { get; set; }
        public string SensorRange { get; set; }
        public string Crew { get; set; }
        public string EncumbranceCapacity { get; set; }
        public string PassengerCapacity { get; set; }
        public string CostRarity { get; set; }
        public string Cost { get; set; }
        public string Rarity { get; set; }
        public string HardPoints { get; set; }
        public string Weapons { get; set; }
        public List<Weapon> WeaponList { get; set; }
        public string Indexes { get; set; }

        /// <summary>
        /// Initializes a new instance of the Transportation class.
        /// </summary>
        public Transportation()
        {
            WeaponList = new List<Weapon>();
        }

        /// <summary>
        /// Initializes a new instance of the Transportation class.
        /// </summary>
        public Transportation(TransportationMin transportMin, HtmlDocument htmlDocument)
        {
            Name = GetSingleNodeText(htmlDocument, "//span[@class='item_name']");
            Silhouette = GetSingleNodeText(htmlDocument, "//div[@class='silhoutte']//span[@class='item_value']");
            Speed = GetSingleNodeText(htmlDocument, "//div[@class='speed']//span[@class='item_value']");
            Handling = GetSingleNodeText(htmlDocument, "//div[@class='handling']//span[@class='item_value']");
            
            string defense = GetSingleNodeText(htmlDocument, "//div[@class='defense']//span[@class='item_value']");
            if (!string.IsNullOrEmpty(defense))
            {
                string[] allDefense = defense.Split('/');
                DefenseFore = allDefense[0];
                DefensePort = allDefense[1];
                DefenseStarboard = allDefense[2];
                DefenseAft = allDefense[3];

                foreach (string defenseSide in allDefense)
                {
                    int specificDefense;
                    if (int.TryParse(defenseSide, out specificDefense ))
                    {
                        DefenseMaximum += specificDefense;
                    }
                }
            }

            Armor = GetSingleNodeText(htmlDocument, "//div[@class='armor']//span[@class='item_value']");
            HullTrauma = GetSingleNodeText(htmlDocument, "//div[@class='hull_trauma']//span[@class='item_value']");
            SystemStrain = GetSingleNodeText(htmlDocument, "//div[@class='system_strain']//span[@class='item_value']");

            var itemDetailNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='item_detail']//span");
            string typeModel = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Type/Model");
            if (typeModel.Contains('/'))
            {
                Type = typeModel.Split('/')[0].Trim();
                Model = typeModel.Split('/')[1].Trim();
            }
            else
            {
                Type = typeModel.Trim();
            }

            Manufacturer = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Manufacturer");
            MaximumAltitude = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Maximum Altitude");
            SensorRange = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Sensor Range");
            Crew = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Crew");
            EncumbranceCapacity = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Encumbrance");
            PassengerCapacity = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Passenger");

            Cost = transportMin.Cost;
            Rarity = transportMin.Rarity;
            CostRarity = String.Format(CultureInfo.InvariantCulture,"{0} credits / {1}", Cost, Rarity);

            HardPoints = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Customization");
            Weapons = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Weapons");
            Indexes = GetSingleNodeTextByNodeCollection(itemDetailNodes, "Indexes");

            WeaponList = TransportationWeaponMapping.Instance.GetWeaponsByTransportationName(Name);
        }

        private static string GetSingleNodeText(HtmlDocument htmlNode, string xpath)
        {
            string text = htmlNode.DocumentNode.SelectSingleNode(xpath).InnerText.Trim();
            return DecodeText(text);
        }

        public static string GetSingleNodeTextByNodeCollection(HtmlNodeCollection collection, string valueName)
        {
            string text = collection.Where(x => x.InnerText.Contains(valueName)).FirstOrDefault().NextSibling.InnerText;
            return DecodeText(text);
        }

        private static string DecodeText(string text)
        {
            text = text.Replace("&quot;", "&#39;");
            return HttpUtility.HtmlDecode(text).Trim();
        }
    }
}
