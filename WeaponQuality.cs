using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace WebCrawler
{
    public class WeaponQuality
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public WeaponQuality(string name)
        {
            Name = name;
            Value = 0;
        }

        public WeaponQuality(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static WeaponQuality GetWeaponQualityByShortcut(string shortcut)
        {
            var number = Regex.Match(shortcut, @"[0-9]+");
            var shortcutWithoutNumber = Regex.Match(shortcut, @"[A-Za-z]+").Value;

            WeaponQuality quality = null;
            switch (shortcutWithoutNumber.ToUpperInvariant())
            {
                case "AF":
                    quality = new WeaponQuality("Autofire");
                    break;
                case "BLA":
                    quality = new WeaponQuality("Blast");
                    break;
                case "BRE":
                    quality = new WeaponQuality("Breach");
                    break;
                case "DIS":
                    quality = new WeaponQuality("Disorient");
                    break;
                case "STUN":
                    quality = new WeaponQuality("Stun Damage");
                    break;
                case "GUI":
                    quality = new WeaponQuality("Guided");
                    break;
                case "LIM":
                    quality = new WeaponQuality("Limited Ammo");
                    break;
                case "SF":
                    quality = new WeaponQuality("Slow Firing");
                    break;
                case "ION":
                    quality = new WeaponQuality("Ion");
                    break;
                case "ACC":
                    quality = new WeaponQuality("Accurate");
                    break;
                case "INACC":
                    quality = new WeaponQuality("Inaccurate");
                    break;
                case "LIN":
                    quality = new WeaponQuality("Linked");
                    break;
                case "TRA":
                    quality = new WeaponQuality("Tractor");
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