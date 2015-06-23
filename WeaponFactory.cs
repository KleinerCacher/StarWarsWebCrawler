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

            string[] mounts = mountShortCut.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mount in mounts)
            {
                weapon.Mounts.Add(new Mount(mount));
            }

            return weapon;
        }

        internal static Weapon CreateOtherText(string[] weaponStats)
        {
            return new WeaponText(weaponStats[1], Weapon.WeaponType.OtherTextInName);
        }
    }
}
