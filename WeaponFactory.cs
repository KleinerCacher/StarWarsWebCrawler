using System;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(string[] weaponStats)
        {
            if (weaponStats.Length == 3)
            {
                return CreateWeapon(weaponStats[0], weaponStats[1], weaponStats[2]);
            }
            else
            {
                // Used when text should be displayed
                if (weaponStats[0].ToUpper().Equals("OTHER"))
                {
                    return WeaponFactory.CreateOtherText(weaponStats);
                }

                return CreateWeapon(weaponStats[0], weaponStats[1]);
            }
        }

        public static Weapon CreateWeapon(
            string mountShortCut,
            string weaponShortcut,
            string additionalCommaSeperatedWeaponQualities)
        {
            Weapon weapon = CreateWeapon(mountShortCut, weaponShortcut);
            if (weapon != null)
            {
                weapon.WeaponQualities =
                           WeaponQuality.AddOrAremoveWeaponQualities(
                               weapon.WeaponQualities, additionalCommaSeperatedWeaponQualities);
            }

            return weapon;
        }

        public static Weapon CreateWeapon(string mountShortCut, string weaponShortcut)
        {
            Weapon weapon = null;
            switch (weaponShortcut.ToUpperInvariant())
            {
                case "AB":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.AutoBlaster);
                    break;
                case "BCL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.BlasterCannonLight);
                    break;
                case "BCH":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.BlasterCannonHeavy);
                    break;
                case "CM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.ConcussionMissile);
                    break;
                case "ICL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.IonCannonLight);
                    break;
                case "ICM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.IonCannonMedium);
                    break;
                case "ICH":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.IonCannonHeavy);
                    break;
                case "ICB":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.IonCannonBattleship);
                    break;
                case "LCL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.LaserCannonLight);
                    break;
                case "LCM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.LaserCannonMedium);
                    break;
                case "LCH":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.LaserCannonHeavy);
                    break;
                case "QLC":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.QuadLaserCannon);
                    break;
                case "TBL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TractorBeamLight);
                    break;
                case "TBM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TractorBeamMedium);
                    break;
                case "TBH":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TractorBeamHeavy);
                    break;
                case "TLL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TurboLaserLight);
                    break;
                case "TLM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TurboLaserMedium);
                    break;
                case "TLH":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.TurboLaserHeavy);
                    break;
                case "PTL":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.ProtonTorpedoLauncher);
                    break;
                case "PBB":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.ProtonBombBay);
                    break;
                case "MDM":
                    weapon = Weapon.GetWeapon(Weapon.WeaponType.MassDriverMissle);
                    break;
                default:
                    throw new ArgumentException("Shortcut not available");
            }

            FillInWeaponMount(weapon, mountShortCut);

            return weapon;
        }

        internal static Weapon CreateOtherText(string[] weaponStats)
        {
            return new WeaponText(weaponStats[1], Weapon.WeaponType.OtherTextInName);
        }

        private static void FillInWeaponMount(Weapon weapon, string mountShortCut)
        {
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four",
                "Five", "Six", "Seven", "Eight", "Nine"};

            const string forward = "Forward";
            const string aft = "Aft";
            const string all = "All";
            const string down = "Down";
            const string portAndStarboard = "Port and Starbord";
            const string portStarboardForward = "Port, Starbord or Forward";
            const string portOrStarboardOrForwardOrAft = "Port or Starbord or Forward or Aft";
            const string descHardpoint = "Hardpoint";
            const string descRetractableForward = "Retractable forward";
            const string descRetractableAft = "Retractable Aft";
            const string descTurretAll = "Turret";
            const string descTurretForward = "Forward, Port and Starbord";
            const string descPortAndStarbordWing = "Port and Starbord wing";
            const string descPortStarboardForward = "Port, starbord and forward";
            const string descPortAndStarboard = "Port and Starbord";
            const string descVentralPortAndStarboard = "Ventral, Port and Starbord";
            const string descPortAndStarboardTurret = "Port and Starbord Turret";
            const string descRetractablePortAndStarboardTurret = "Retractable Port and Starbord Turret";
            const string descRetractablePortStarboardForwardAftTurret = "Retractable Port, Starbord, Forward and Aft Turret";
            const string descTDorsal = "Dorsal Turret";
            const string descTRetractableDorsal = "Retractable dorsal turret";
            const string descVentral = "Ventral";
            const string descTVentral = "Ventral Turret";
            const string descDorsalAndVentral = "Dorsal and Ventral Turret";

            int numberOfWeapons = -1;
            if (Char.IsDigit(mountShortCut[0]))
            {
                var number = Regex.Match(mountShortCut, @"([0-9]+)[a-zA-Z]").Groups[1].Value;
                mountShortCut = Regex.Match(mountShortCut, @"[0-9]+(.*)").Groups[1].Value;

                numberOfWeapons = int.Parse(number.ToString());
            }

            int numberOfLinkedWeapons = -1;
            if (Char.IsDigit(mountShortCut[mountShortCut.Length - 1]))
            {
                numberOfLinkedWeapons = int.Parse(mountShortCut[mountShortCut.Length - 1].ToString());
                mountShortCut = mountShortCut.Remove(mountShortCut.Length - 1);
            }

            string fireArc = string.Empty;
            string description = string.Empty;
            switch (mountShortCut.ToUpperInvariant())
            {
                case "FO":
                    fireArc = forward;
                    description = forward;
                    break;
                case "REFO":
                    fireArc = forward;
                    description = descRetractableForward;
                    break;
                case "AF":
                    fireArc = aft;
                    description = aft;
                    break;
                case "REAF":
                    fireArc = aft;
                    description = descRetractableAft;
                    break;
                case "TFO":
                    fireArc = forward;
                    description = descTurretForward;
                    break;
                case "TALL":
                    fireArc = all;
                    description = descTurretAll;
                    break;
                case "HAFO":
                    fireArc = forward;
                    description = descHardpoint;
                    break;
                case "POSTWINFO":
                    fireArc = forward;
                    description = descPortAndStarbordWing;
                    break;
                case "TDO":
                    fireArc = all;
                    description = descTDorsal;
                    break;
                case "TREDO":
                    fireArc = all;
                    description = descTRetractableDorsal;
                    break;
                case "VEN":
                    fireArc = down;
                    description = descVentral;
                    break;
                case "TVEN":
                    fireArc = all;
                    description = descTVentral;
                    break;
                case "TDOVEN":
                    fireArc = all;
                    description = descDorsalAndVentral;
                    break;
                case "POST":
                    fireArc = portAndStarboard;
                    description = descPortAndStarboard;
                    break;
                case "POSTVEN":
                    fireArc = all;
                    description = descVentralPortAndStarboard;
                    break;
                case "TPOST":
                    fireArc = portAndStarboard;
                    description = descPortAndStarboardTurret;
                    break;
                case "POSTFO":
                    fireArc = portStarboardForward;
                    description = descPortStarboardForward;
                    break;
                case "TREPOSTSI":
                    fireArc = portAndStarboard;
                    description = descRetractablePortAndStarboardTurret;
                    break;
                case "TREPOSTFOAF":
                    fireArc = portOrStarboardOrForwardOrAft;
                    description = descRetractablePortStarboardForwardAftTurret;
                    break;
                default:
                    throw new ArgumentException("Fire Arc Shortcut not availaible");
            }

            string numberOfWeaponsString = string.Empty;
            if (numberOfWeapons != -1)
            {
                if (numberOfWeapons > unitsMap.Length - 1)
                {
                    numberOfWeaponsString = numberOfWeapons + " ";
                }
                else
                {
                    numberOfWeaponsString = unitsMap[numberOfWeapons] + " ";
                }

                description = description.ToLower();
            }

            string numberOfLinkedWeaponsString = string.Empty;
            if (numberOfLinkedWeapons != -1)
            {
                switch (numberOfLinkedWeapons)
                {
                    case 2:
                        numberOfLinkedWeaponsString += " twin";
                        break;
                    case 3:
                        numberOfLinkedWeaponsString += " triple";
                        break;
                    default:
                        throw new ArgumentException("Number Of Linked Weapons Not Available");
                }
            }

            weapon.FireArcDescription = numberOfWeaponsString + description + " mounted" + numberOfLinkedWeaponsString;
            weapon.FireArc = fireArc;
        }
    }
}
