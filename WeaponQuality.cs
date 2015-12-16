using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace WebCrawler
{
    [Serializable]
    public class WeaponQuality
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Iconstring { get; set; }

        public WeaponQuality() { }

        public WeaponQuality(string name, string iconString)
        {
            Name = name;
            Value = 0;
            Iconstring = iconString;
        }

        public static WeaponQuality GetWeaponQualityByShortcut(string shortcut)
        {
            var number = Regex.Match(shortcut, @"[0-9]+");
            var shortcutWithoutNumber = Regex.Match(shortcut, @"[A-Za-z]+").Value;

            WeaponQuality quality = null;
            switch (shortcutWithoutNumber.ToUpperInvariant())
            {
                case "AF":
                    quality = new WeaponQuality("Autofire", "(autofire)");
                    break;
                case "BLA":
                    quality = new WeaponQuality("Blast", "(blast)");
                    break;
                case "BRE":
                    quality = new WeaponQuality("Breach", "(breach)");
                    break;
                case "DIS":
                    quality = new WeaponQuality("Disorient", "(disorient)");
                    break;
                case "STUN":
                    quality = new WeaponQuality("Stun Damage", "(stundamage)");
                    break;
                case "GUI":
                    quality = new WeaponQuality("Guided", "(guided)");
                    break;
                case "LIM":
                    quality = new WeaponQuality("Limited Ammo", "(limitedammo)");
                    break;
                case "SF":
                    quality = new WeaponQuality("Slow Firing", "(slowfiring)");
                    break;
                case "ION":
                    quality = new WeaponQuality("Ion", "(ion)");
                    break;
                case "ACC":
                    quality = new WeaponQuality("Accurate", "(accurate)");
                    break;
                case "INACC":
                    quality = new WeaponQuality("Inaccurate", "(inaccurate)");
                    break;
                case "LIN":
                    quality = new WeaponQuality("Linked", "(linked)");
                    break;
                case "TRA":
                    quality = new WeaponQuality("Tractor", "(tractor)");
                    break;
                case "VIC":
                    quality = new WeaponQuality("Vicious", "(vicious)");
                    break;
                case "KNO":
                    quality = new WeaponQuality("Knockdown", "(knockdown)");
                    break;
                case "PIER":
                    quality = new WeaponQuality("Pierce", "(pierce)");
                    break;
                default:
                    throw new ArgumentException("WeaponQuality - Shortcut not available");
            }

            if (quality != null && number.Success)
            {
                quality.Value = int.Parse(number.Value);
            }

            return quality;
        }

        internal static List<WeaponQuality> GetWeaponQualitiesByListOfShortcuts(string commaSeperatedShortcuts)
        {
            List<WeaponQuality> qualities = new List<WeaponQuality>();
            return AddOrAremoveWeaponQualities(qualities, commaSeperatedShortcuts);
        }

        internal static List<WeaponQuality> AddOrAremoveWeaponQualities(
                                                List<WeaponQuality> qualities,
                                                string commaSeperatedShortcuts)
        {
            string[] splittedShortcuts =
                commaSeperatedShortcuts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string shortcut in splittedShortcuts)
            {
                bool shouldDelete = false;
                if (shortcut[0] == '-')
                {
                    var removeQuality = GetWeaponQualityByShortcut(shortcut.Substring(1));
                    qualities.RemoveAll(q => q.Name == removeQuality.Name);
                }
                else
                {
                    qualities.Add(GetWeaponQualityByShortcut(shortcut));
                }
            }

            return qualities;
        }
    }
}